window.filterGallery = function (modalId) {
    const q = (document.getElementById(modalId + '-search')?.value || '').trim().toLowerCase();
    const grid = document.getElementById(modalId + '-grid');
    if (!grid) return;
    grid.querySelectorAll('.gallery-card').forEach(card => {
        const match = (card.dataset.key || '').toLowerCase().includes(q);
        card.classList.toggle('d-none', !match);
    });
};

window.selectFromGallery = function (modalId, key, url) {
    const input = document.getElementById('SelectedKey');
    if (input) input.value = key;
    const preview = document.getElementById('imagePreview');
    if (preview && url) preview.src = url;
    const el = document.getElementById(modalId);
    if (el && window.bootstrap) window.bootstrap.Modal.getOrCreateInstance(el).hide();
};
