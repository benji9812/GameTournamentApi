const API = 'http://localhost:5116/api';

function showFeedback(id, msg, isError) {
    const el = document.getElementById(id);
    el.textContent = msg;
    el.className = 'feedback show ' + (isError ? 'err' : 'ok');
    setTimeout(() => el.className = 'feedback', 3500);
}

function toLocalDatetimeValue(isoString) {
    return new Date(isoString).toISOString().slice(0, 16);
}

function escHtml(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

function escJs(str) {
    return String(str)
        .replace(/\\/g, '\\\\')
        .replace(/'/g, "\\'");
}

function formatErrors(err) {
    if (err?.errors) return Object.values(err.errors).flat().join(' ');
    if (err?.title) return err.title;
    return '';
}

async function loadTournaments() {
    const search = document.getElementById('search-input').value.trim();
    const url = search
        ? `${API}/Tournaments?search=${encodeURIComponent(search)}`
        : `${API}/Tournaments`;

    try {
        const res = await fetch(url);
        if (!res.ok) throw new Error('Kunde inte hämta turneringar');
        const data = await res.json();
        renderTournaments(data);
        populateTournamentSelect(data);
    } catch (e) {
        document.getElementById('tournament-list').innerHTML =
            `<li class="empty">❌ ${e.message}</li>`;
    }
}

function renderTournaments(tournaments) {
    const list = document.getElementById('tournament-list');
    if (!tournaments.length) {
        list.innerHTML = '<li class="empty">Inga turneringar hittades.</li>';
        return;
    }
    list.innerHTML = '';
    tournaments.forEach((t, index) => {
        const li = document.createElement('li');
        li.className = 'tournament-card';

        const gamesHtml = t.games?.length
            ? t.games.map(g => `
                <div class="game-item">
                  <div>
                    <strong class="rainbow-text">${escHtml(g.title)}</strong>
                    <span> &nbsp;${new Date(g.time).toLocaleString('sv-SE')}</span>
                  </div>
                  <button class="btn btn-danger" onclick="deleteGame(${g.id})">Ta bort</button>
                </div>`).join('')
            : '<p style="font-size:0.8rem;color:var(--muted);margin-top:0.4rem">Inga matcher.</p>';

        li.innerHTML = `
            <h3 class="rainbow-text">${escHtml(t.title)}</h3>
            <p>${escHtml(t.description)}</p>
            <p>📅 ${new Date(t.startDate).toLocaleString('sv-SE')} &nbsp;|&nbsp; 👥 Max ${t.maxPlayers} spelare</p>
            <div class="card-actions">
              <button class="btn btn-warning"
                onclick="openEdit(${t.id},'${escJs(t.title)}','${escJs(t.description)}','${t.startDate}',${t.maxPlayers})">
                Redigera
              </button>
              <button class="btn btn-danger" onclick="deleteTournament(${t.id})">Ta bort</button>
            </div>
            <div class="games-section">
              <strong class="rainbow-text">Matcher:</strong>
              ${gamesHtml}
            </div>`;
        list.appendChild(li);

        setTimeout(() => li.classList.add('card-show'), 80 * index);
    });
}

function populateTournamentSelect(tournaments) {
    const sel = document.getElementById('g-tournament');
    sel.innerHTML = tournaments.length
        ? tournaments.map(t => `<option value="${t.id}">${escHtml(t.title)}</option>`).join('')
        : '<option value="">– Inga turneringar –</option>';
}

document.getElementById('create-tournament-form')
    .addEventListener('submit', async e => {
        e.preventDefault();
        const body = {
            title: document.getElementById('t-title').value.trim(),
            description: document.getElementById('t-desc').value.trim(),
            startDate: document.getElementById('t-date').value,
            maxPlayers: parseInt(document.getElementById('t-players').value)
        };
        try {
            const res = await fetch(`${API}/Tournaments`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            if (!res.ok) {
                const err = await res.json().catch(() => ({}));
                throw new Error(formatErrors(err) || `HTTP ${res.status}`);
            }
            showFeedback('create-t-feedback', '✅ Turneringen skapades!', false);
            e.target.reset();
            loadTournaments();
        } catch (err) {
            showFeedback('create-t-feedback', '❌ ' + err.message, true);
        }
    });

async function deleteTournament(id) {
    if (!confirm('Ta bort turneringen?')) return;
    try {
        const res = await fetch(`${API}/Tournaments/${id}`, { method: 'DELETE' });
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        loadTournaments();
    } catch (e) {
        alert('❌ Kunde inte ta bort: ' + e.message);
    }
}

function openEdit(id, title, desc, startDate, maxPlayers) {
    document.getElementById('edit-id').value = id;
    document.getElementById('edit-title').value = title;
    document.getElementById('edit-desc').value = desc;
    document.getElementById('edit-date').value = toLocalDatetimeValue(startDate);
    document.getElementById('edit-players').value = maxPlayers;
    document.getElementById('edit-modal').classList.add('open');
}

function closeModal() {
    document.getElementById('edit-modal').classList.remove('open');
}

async function submitEdit() {
    const id = document.getElementById('edit-id').value;
    const body = {
        title: document.getElementById('edit-title').value.trim(),
        description: document.getElementById('edit-desc').value.trim(),
        startDate: document.getElementById('edit-date').value,
        maxPlayers: parseInt(document.getElementById('edit-players').value)
    };
    try {
        const res = await fetch(`${API}/Tournaments/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });
        if (!res.ok) {
            const err = await res.json().catch(() => ({}));
            throw new Error(formatErrors(err) || `HTTP ${res.status}`);
        }
        showFeedback('edit-feedback', '✅ Sparat!', false);
        setTimeout(() => { closeModal(); loadTournaments(); }, 800);
    } catch (err) {
        showFeedback('edit-feedback', '❌ ' + err.message, true);
    }
}

document.getElementById('create-game-form')
    .addEventListener('submit', async e => {
        e.preventDefault();
        const tid = document.getElementById('g-tournament').value;
        if (!tid) {
            showFeedback('create-g-feedback', '❌ Välj en turnering först.', true);
            return;
        }
        const body = {
            title: document.getElementById('g-title').value.trim(),
            time: document.getElementById('g-time').value,
            tournamentId: parseInt(tid)
        };
        try {
            const res = await fetch(`${API}/Games`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            if (!res.ok) {
                const err = await res.json().catch(() => ({}));
                throw new Error(formatErrors(err) || `HTTP ${res.status}`);
            }
            showFeedback('create-g-feedback', '✅ Matchen lades till!', false);
            e.target.reset();
            loadTournaments();
        } catch (err) {
            showFeedback('create-g-feedback', '❌ ' + err.message, true);
        }
    });

async function deleteGame(id) {
    if (!confirm('Ta bort matchen?')) return;
    try {
        const res = await fetch(`${API}/Games/${id}`, { method: 'DELETE' });
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        loadTournaments();
    } catch (e) {
        alert('❌ Kunde inte ta bort match: ' + e.message);
    }
}

// Canvas-bakgrund
const canvas = document.getElementById('bgCanvas');
const ctx = canvas.getContext('2d');
let w, h, particles;

function initCanvas() {
    w = canvas.width = window.innerWidth;
    h = canvas.height = window.innerHeight;
    particles = [];
    for (let i = 0; i < 80; i++) {
        particles.push({
            x: Math.random() * w,
            y: Math.random() * h,
            vx: (Math.random() - 0.5) * 0.6,
            vy: (Math.random() - 0.5) * 0.6,
            radius: Math.random() * 2 + 1,
            color: Math.random() > 0.5 ? 'rgba(0,240,255,0.8)' : 'rgba(255,0,230,0.8)'
        });
    }
}

function drawCanvas() {
    ctx.clearRect(0, 0, w, h);
    for (const p of particles) {
        ctx.beginPath();
        ctx.arc(p.x, p.y, p.radius, 0, Math.PI * 2);
        ctx.fillStyle = p.color;
        ctx.fill();
        p.x += p.vx;
        p.y += p.vy;
        if (p.x < 0 || p.x > w) p.vx *= -1;
        if (p.y < 0 || p.y > h) p.vy *= -1;
    }
    for (let i = 0; i < particles.length; i++) {
        for (let j = i + 1; j < particles.length; j++) {
            const dx = particles[i].x - particles[j].x;
            const dy = particles[i].y - particles[j].y;
            const dist = Math.sqrt(dx * dx + dy * dy);
            if (dist < 120) {
                ctx.beginPath();
                ctx.strokeStyle = `rgba(255,221,0,${1 - dist / 120})`;
                ctx.lineWidth = 1;
                ctx.moveTo(particles[i].x, particles[i].y);
                ctx.lineTo(particles[j].x, particles[j].y);
                ctx.stroke();
            }
        }
    }
    requestAnimationFrame(drawCanvas);
}

function initPanelFadeIn() {
    const panels = document.querySelectorAll('.panel, .modal');
    panels.forEach((p, index) => {
        setTimeout(() => p.classList.add('show-panel'), 150 + index * 130);
    });
}

window.addEventListener('resize', initCanvas);

initCanvas();
drawCanvas();
initPanelFadeIn();
loadTournaments();
