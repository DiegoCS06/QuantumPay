document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("create-account-form");
    const userTypeSelect = document.getElementById("UserType");

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

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const userType = userTypeSelect.value;
        let data = {};
        let apiUrl = "";
        //CLIENTE
        if (userType === "Cliente") {
            const fotoCedulaFile = document.getElementById("FotoCedula").files[0];
            const fotoPerfilFile = document.getElementById("FotoPerfil").files[0];

            if (!fotoCedulaFile || !fotoPerfilFile) {
                return Swal.fire({
                    icon: 'warning',
                    title: 'Faltan fotos',
                    text: 'Por favor, suba ambas fotos (cédula y perfil) antes de continuar.'
                });
            }

            function toBase64(file) {
                return new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = () => {
                        const base64String = reader.result.split(",")[1];
                        resolve(base64String);
                    };
                    reader.onerror = error => reject(error);
                });
            }

            const fotoCedulaBase64 = await toBase64(fotoCedulaFile);
            const fotoPerfilBase64 = await toBase64(fotoPerfilFile);

            data = {
                nombre: form.querySelector('[name="SignUpRequest.Nombre"]').value,
                apellido: form.querySelector('[name="SignUpRequest.Apellido"]').value,
                cedula: form.querySelector('[name="SignUpRequest.Cedula"]').value,
                telefono: form.querySelector('[name="SignUpRequest.Telefono"]').value,
                correo: form.querySelector('[name="SignUpRequest.Correo"]').value,
                direccion: form.querySelector('[name="SignUpRequest.Direccion"]').value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value,
                IBAN: form.querySelector('[name="SignUpRequest.IBAN"]').value,
                fotoCedula: fotoCedulaBase64,
                fotoPerfil: fotoPerfilBase64,
                fechaNacimiento: form.querySelector('[name="SignUpRequest.FechaNacimiento"]').value
            };

            await fetch("https://localhost:5001/api/Cliente/SendSmsVerification?telefono=" + encodeURIComponent(data.telefono));
            await fetch("https://localhost:5001/api/Cliente/SendEmailVerification?email=" + encodeURIComponent(data.correo));

            const { value: smsCode } = await Swal.fire({
                title: 'Código SMS',
                text: 'Ingrese el código recibido por SMS',
                input: 'text',
                inputPlaceholder: 'Código de 6 dígitos',
                showCancelButton: true,
                confirmButtonText: 'Confirmar'
            });
            if (!smsCode) return Swal.fire('Cancelado', 'No ingresó el código SMS.', 'error');

            const { value: emailCode } = await Swal.fire({
                title: 'Código Email',
                text: 'Ingrese el código recibido por Email',
                input: 'text',
                inputPlaceholder: 'Código de 6 dígitos',
                showCancelButton: true,
                confirmButtonText: 'Confirmar'
            });
            if (!emailCode) return Swal.fire('Cancelado', 'No ingresó el código de email.', 'error');

            apiUrl = `https://localhost:5001/api/Cliente/Create?emailCode=${encodeURIComponent(emailCode)}&smsCode=${encodeURIComponent(smsCode)}`;
        //ADMIN
        } else if (userType === "Admin") {
            data = {
                nombreUsuario: form.querySelector('[name="SignUpRequest.NombreUsuario"]').value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value
            };
            apiUrl = "https://localhost:5001/api/Admin/Create";
        //COMERCIO
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
        //BANCO
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
            return Swal.fire({
                icon: 'error',
                title: 'Tipo inválido',
                text: 'Seleccione un tipo de usuario válido.'
            });
        }

        //SE LANZA DESPUES DE SUBMIT EN TODO TIPO DE USUARIO PARA EL AWAIT
        Swal.fire({
            title: 'Verificando códigos, solicitud y fotografías...',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        try {
            const response = await fetch(apiUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            Swal.close(); 
            //ALERTAS PARA REGISTRO EXITOSO O ERRONEO
            if (response.ok) {
                if (userType === "Cliente") {
                    await fetch("https://localhost:5001/api/Cliente/SendPhoneVerificationCode", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ telefono: data.telefono })
                    });
                    await fetch("https://localhost:5001/api/Cliente/SendEmailVerificationCode", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ correo: data.correo })
                    });
                }

                await Swal.fire({
                    icon: 'success',
                    title: 'Registro exitoso',
                    text: 'Ahora puede iniciar sesión.',
                    confirmButtonText: 'Aceptar'
                });
                window.location.href = "/Login";
            } else {
                const error = await response.text();
                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar el usuario',
                    text: error
                });
            }
        } catch (err) {
            Swal.close();
            Swal.fire({
                icon: 'error',
                title: 'Error de red o del servidor',
                text: err.message || ''
            });
        }

    });

});
