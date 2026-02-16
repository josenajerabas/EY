
        // Loading state en el botón
    document.getElementById('loginForm').addEventListener('submit', function(e) {
            const btn = document.getElementById('btnLogin');
    btn.disabled = true;
    btn.innerHTML = '<span class="spinner"></span> Ingresando...';
        });

    // Auto-focus en el primer input
    window.addEventListener('load', function() {
        document.querySelector('input[name="NombreUsuario"]').focus();
        });
