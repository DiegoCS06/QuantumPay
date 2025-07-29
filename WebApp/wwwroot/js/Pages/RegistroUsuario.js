document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("create-account-form");
    const userTypeSelect = document.getElementById("UserType");

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const userType = userTypeSelect.value;

        let data = {};
        let apiUrl = "";

        if (userType === "Cliente") {
            data = {
                nombre: form.querySelector('[name="SignUpRequest.Nombre"]').value,
                apellido: form.querySelector('[name="SignUpRequest.Apellido"]').value,
                cedula: form.querySelector('[name="SignUpRequest.Cedula"]').value,
                telefono: parseInt(form.querySelector('[name="SignUpRequest.Telefono"]').value),
                correo: form.querySelector('[name="SignUpRequest.Correo"]').value,
                direccion: form.querySelector('[name="SignUpRequest.Direccion"]').value,
                contrasena: form.querySelector('[name="SignUpRequest.Password"]').value,
                IBAN: form.querySelector('[name="SignUpRequest.IBAN"]').value,
                fotoCedula: "",
                fotoPerfil: "",
                fechaNacimiento: form.querySelector('[name="SignUpRequest.FechaNacimiento"]').value
            };
            apiUrl = "https://localhost:5001/api/Cliente/Create";
        } else if (userType === "Admin") {
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
