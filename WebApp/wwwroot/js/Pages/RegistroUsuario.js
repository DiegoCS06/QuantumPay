document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("create-account-form");
    const userTypeSelect = document.getElementById("UserType");

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const userType = userTypeSelect.value;

        let data = {};
        let apiUrl = "";

        //para tipo de usuario Cliente, se requiere subir fotos de cédula y perfil
        if (userType === "Cliente") {

            // subir fotos de cédula y perfil
            const fotoCedulaFile = document.getElementById("FotoCedula").files[0];
            const fotoPerfilFile = document.getElementById("FotoPerfil").files[0];
            // Validar que ambas fotos estén seleccionadas
            if (!fotoCedulaFile || !fotoPerfilFile) {

                alert("Por favor, suba ambas fotos (Cédula y Perfil).");
                return;

            }
            // Función para convertir archivo a base64
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

            // Convertir ambas imágenes a base64 para enviar a Rekognition
            console.log("Convirtiendo foto de cédula a base64...");
            const fotoCedulaBase64 = await toBase64(fotoCedulaFile);
            console.log("Foto de cédula convertida.");

            console.log("Convirtiendo foto de perfil a base64...");
            const fotoPerfilBase64 = await toBase64(fotoPerfilFile);
            console.log("Foto de perfil convertida.");


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
            // Step 1: Send verification codes
            await fetch("https://localhost:5001/api/Cliente/SendSmsVerification?telefono=" + encodeURIComponent(data.telefono));
            await fetch("https://localhost:5001/api/Cliente/SendEmailVerification?email=" + encodeURIComponent(data.correo));

            // Step 2: Prompt user for codes
            const smsCode = prompt("Ingrese el código de verificación recibido por SMS:");
            const emailCode = prompt("Ingrese el código de verificación recibido por Email:");

            // Step 3: Add codes as query parameters
            apiUrl = `https://localhost:5001/api/Cliente/Create?emailCode=${encodeURIComponent(emailCode)}&smsCode=${encodeURIComponent(smsCode)}`;
        } else if (userType === "Admin") { //para tipo de usuario Admin, no se requiere subir fotos
            data = {
                nombreUsuario: form.querySelector('[name="SignUpRequest.NombreUsuario"]').value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value
            };
            apiUrl = "https://localhost:5001/api/Admin/Create";
        } else if (userType === "CuentaComercio") { //para tipo de usuario CuentaComercio, no se requiere subir fotos
            data = {
                nombreUsuario: document.getElementById("SignUpRequest_NombreUsuario_CuentaComercio").value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value,
                cedulaJuridica: document.getElementById("SignUpRequest_CedulaJuridica_CuentaComercio").value,
                telefono: parseInt(document.getElementById("SignUpRequest_Telefono_CuentaComercio").value) || 0,
                correoElectronico: document.getElementById("SignUpRequest_CorreoElectronico_CuentaComercio").value,
                direccion: document.getElementById("SignUpRequest_Direccion_CuentaComercio").value
            };
            apiUrl = "https://localhost:5001/api/CuentaComercio/Create";
        } else if (userType === "InstitucionBancaria") { //para tipo de usuario InstitucionBancaria, no se requiere subir fotos
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
                if (userType === "Cliente") {
                    // Send verification code to phone
                    await fetch("https://localhost:5001/api/Cliente/SendPhoneVerificationCode", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ telefono: data.telefono })
                    });

                    // Send verification code to email
                    await fetch("https://localhost:5001/api/Cliente/SendEmailVerificationCode", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ correo: data.correo })
                    });
                }

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
