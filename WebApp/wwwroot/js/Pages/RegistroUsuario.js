document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("create-account-form");
    const userTypeSelect = document.getElementById("UserType");

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const userType = userTypeSelect.value;

        let data = {};
        let apiUrl = "";

        // Solo para Cliente: realizar verificaciones previas
        if (userType === "Cliente") {
            // 1. Obtener datos del formulario
            const nombre = form.querySelector('[name="SignUpRequest.Nombre"]').value;
            const apellido = form.querySelector('[name="SignUpRequest.Apellido"]').value;
            const cedula = form.querySelector('[name="SignUpRequest.Cedula"]').value;
            const telefono = form.querySelector('[name="SignUpRequest.Telefono"]').value;
            const correo = form.querySelector('[name="SignUpRequest.Correo"]').value;
            const contrasena = form.querySelector('[name="SignUpRequest.Password"]').value;
            const confirmarContrasena = form.querySelector('[name="SignUpRequest.ConfirmPassword"]')?.value;
            const fotoCedulaInput = form.querySelector('[name="SignUpRequest.FotoCedula"]');
            const fotoPerfilInput = form.querySelector('[name="SignUpRequest.FotoPerfil"]');
            const fotoCedula = fotoCedulaInput?.files?.[0];
            const fotoPerfil = fotoPerfilInput?.files?.[0];

            // Validar contraseñas
            if (confirmarContrasena && contrasena !== confirmarContrasena) {
                alert('Las contraseñas no coinciden.');
                return;
            }

            // 2. Enviar OTPs
            try {
                await Promise.all([
                    fetch(`/api/Cliente/SendEmailVerification?email=${encodeURIComponent(correo)}`),
                    fetch(`/api/Cliente/SendSmsVerification?telefono=${encodeURIComponent(telefono)}`)
                ]);
            } catch (err) {
                alert('Error al enviar códigos OTP.');
                return;
            }

            // 3. Pedir códigos al usuario
            const emailCode = prompt('Ingrese el código recibido por correo:');
            if (!emailCode) return;
            const smsCode = prompt('Ingrese el código recibido por SMS:');
            if (!smsCode) return;

            // 4. Convertir imágenes a base64 (si existen)
            async function fileToBase64(file) {
                return new Promise((resolve, reject) => {
                    if (!file) return resolve("");
                    const reader = new FileReader();
                    reader.onload = () => resolve(reader.result.split(',')[1]);
                    reader.onerror = reject;
                    reader.readAsDataURL(file);
                });
            }
            const fotoCedulaBase64 = await fileToBase64(fotoCedula);
            const fotoPerfilBase64 = await fileToBase64(fotoPerfil);

            // 5. Preparar datos y endpoint
            data = {
                nombre,
                apellido,
                cedula,
                telefono,
                correo,
                direccion: form.querySelector('[name="SignUpRequest.Direccion"]').value,
                contrasena,
                IBAN: form.querySelector('[name="SignUpRequest.IBAN"]').value,
                fotoCedula: fotoCedulaBase64,
                fotoPerfil: fotoPerfilBase64,
                fechaNacimiento: form.querySelector('[name="SignUpRequest.FechaNacimiento"]').value
            };
            apiUrl = `https://localhost:5001/api/Cliente/Create?emailCode=${encodeURIComponent(emailCode)}&smsCode=${encodeURIComponent(smsCode)}`;
        }
        // Otros tipos de usuario (sin verificaciones)
        else if (userType === "Admin") {
            data = {
                nombreUsuario: form.querySelector('[name="SignUpRequest.NombreUsuario"]').value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value
            };
            apiUrl = "https://localhost:5001/api/Admin/Create";
        } else if (userType === "CuentaComercio") {
            data = {
                nombreUsuario: document.getElementById("SignUpRequest_NombreUsuario_CuentaComercio").value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value,
                cedulaJuridica: document.getElementById("SignUpRequest_CedulaJuridica_CuentaComercio").value,
                telefono: parseInt(document.getElementById("SignUpRequest_Telefono_CuentaComercio").value) || 0,
                correoElectronico: document.getElementById("SignUpRequest_CorreoElectronico_CuentaComercio").value,
                direccion: document.getElementById("SignUpRequest_Direccion_CuentaComercio").value
            };
            apiUrl = "https://localhost:5001/api/CuentaComercio/Create";
        } else if (userType === "InstitucionBancaria") {
            data = {
                codigoIdentidad: document.getElementById("SignUpRequest_CodigoIdentidad").value,
                cedulaJuridica: document.getElementById("SignUpRequest_CedulaJuridica_InstitucionBancaria").value,
                direccionSedePrincipal: document.getElementById("SignUpRequest_DireccionSedePrincipal").value,
                telefono: parseInt(document.getElementById("SignUpRequest_Telefono_InstitucionBancaria").value, 10),
                correoElectronico: document.getElementById("SignUpRequest_CorreoElectronico_InstitucionBancaria").value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value,
                estadoSolicitud: "Pendiente"
            };
            apiUrl = "https://localhost:5001/api/InstitucionBancaria/Create";
        } else {
            alert("Seleccione un tipo de usuario válido.");
            return;
        }

        try {
            const response = await fetch(apiUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Registro exitoso. Ahora puede iniciar sesión.");
                window.location.href = "/Login";
            } else {
                const error = await response.text();
                alert("Error al registrar el usuario: " + error);
            }
        } catch (err) {
            alert("Error de red o del servidor.");
        }
    });

    const allFields = document.querySelectorAll(".user-fields");

    function showFieldsForType(type) {
        allFields.forEach(f => f.style.display = "none");
        if (!type) return;
        const className = "user-" + type.toLowerCase();
        const fields = document.querySelectorAll("." + className);
        fields.forEach(f => f.style.display = "block");
    }

    userTypeSelect.addEventListener("change", function () {
        showFieldsForType(this.value);
    });

    showFieldsForType(userTypeSelect.value);
});
