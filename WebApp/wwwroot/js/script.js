
async function buscarCodigo() {
    const codigoInput = document.getElementById("codigo");
    const nombreInput = document.getElementById("nombre");
    if (codigoInput.value.trim() === "") {
        // Agrega la clase de invalidación
        codigoInput.classList.add("is-invalid");
        return;
    } else {
        // Remueve la clase de invalidación si existe
        codigoInput.classList.remove("is-invalid");
        // Realizar una solicitud al servidor para verificar la existencia del código
        // const response = await fetch(`/api/articulos/${codigoInput.value}`);
        const response = await fetch(`https://localhost:7228/api/Facturacion/${codigoInput.value}`);


        try {
            // Verificar si la respuesta está vacía o no es JSON válido
            if (!response.ok) {
                // Manejar el caso de error
                if (response.status === 404) {
                    // El código no existe
                    nombreInput.value = "";
                    Swal.fire({
                        icon: 'info',
                        title: 'El código no existe en la base de datos.',
                        text: 'Por favor, inténtelo nuevamente.',
                    })
                } else {
                    // Otro error, mostrar mensaje de error genérico
                    Swal.fire({
                        icon: 'info',
                        title: 'Hubo un error al procesar la solicitud.',
                        text: 'Por favor, inténtelo nuevamente.',
                    })
                }
                return;
            }

            // Analizar la respuesta JSON
            data1 = await response.json();

            // Si el código existe, llenar el campo de nombre
            nombreInput.value = data1.art.nombre;
        } catch (error) {
            console.error("Error al analizar la respuesta JSON:", error);
        }
    }
}

let sumaTotal = 0.00;

function agregarArticulo() {
    const codigoInput = document.getElementById("codigo");
    const nombreInput = document.getElementById("nombre");
    const cantidadInput = document.getElementById("cantidad");
    if (codigoInput.value.trim() === "" || nombreInput.value.trim() === "" || cantidadInput.value.trim() === "") {
        // Agrega la clase de invalidación
        codigoInput.classList.add("is-invalid");
        nombreInput.classList.add("is-invalid");
        cantidadInput.classList.add("is-invalid");
        return;
    } else {
        codigoInput.classList.remove("is-invalid");
        nombreInput.classList.remove("is-invalid");
        cantidadInput.classList.remove("is-invalid");
        // Validar que el código exista y que la cantidad esté llena
        if (codigoInput.value && nombreInput.value && cantidadInput.value && data1) {
            // Crear una fila para la tabla
            if (data1.art.iva === true) {
                data1.art.iva = data1.art.precio * 0.13
                data1.art.total = (data1.art.precio + data1.art.iva) * cantidadInput.value
            } else {
                data1.art.iva = 0.00
                data1.art.total = data1.art.precio * cantidadInput.value
            }
            // Obtener la tabla
            const tabla = document.getElementById("tablaArticulos").getElementsByTagName('tbody')[0];

            // Crear una fila para la tabla
            const fila = tabla.insertRow(tabla.rows.length);

            // Agregar celdas y asignar valores directamente a la fila
            fila.insertCell().textContent = data1.art.codigo;
            fila.insertCell().textContent = nombreInput.value;
            fila.insertCell().textContent = data1.art.precio;
            fila.insertCell().textContent = data1.art.iva;
            fila.insertCell().textContent = cantidadInput.value;
            fila.insertCell().textContent = data1.art.total;
            fila.insertCell().textContent = data1.art.id;
            fila.cells[fila.cells.length - 1].style.display = "none";

            // Actualizar la suma total recorriendo todas las filas
            sumaTotal += parseFloat(data1.art.total);

            // Mostrar la suma total
            document.getElementById("sumaTotal").textContent =sumaTotal.toFixed(2);



            // Limpiar los campos del formulario
            codigoInput.value = "";
            nombreInput.value = "";
            cantidadInput.value = "";
        } else {
            alert("Por favor, complete todos los campos antes de agregar.");
        }
    }
}

function enviarFactura() {
    const tabla = document.getElementById("tablaArticulos");
    const filas = tabla.getElementsByTagName('tbody')[0].getElementsByTagName('tr');

    // Crear objetos para el encabezado y los detalles
    const encabezado = {
        Cliente: 1,  
        Fecha: new Date(),
        
    };

    const detalles = [];

    // Recorrer filas de la tabla
    for (let i = 0; i < filas.length; i++) {
        const celdas = filas[i].getElementsByTagName('td');

        // Crear objeto para cada detalle
        const detalle = {
            
            CodArticulo: celdas[6].textContent,  
            Cantidad: celdas[4].textContent, 
            Total: celdas[5].textContent,  
        };

        detalles.push(detalle);
    }

    // Crear objeto para la factura completa
    const factura = {
        Cliente: 1,
        Fecha: new Date(),
        Detalles: detalles,
    };
    console.log('Objeto factura:', factura);
    // Realizar la solicitud al controlador
   fetch('/Facturacion/GuardarFactura', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
       body: JSON.stringify(factura),
   }).then(response => response.json())
       .then(data => {
           if (data.success) {
               // Factura guardada con éxito, redirigir
               
               var generarPdfButton = document.getElementById('GenPdf');
               generarPdfButton.click();
               setTimeout(function () {
                   // Recargar la página después de esperar 10 segundos
                   location.reload();
               }, 10000); // 10000 milisegundos = 10 segundos
           } else {
               // Mostrar algún mensaje de error al usuario
               console.error(data.message);
           }
       })
       .catch(error => {
           console.error('Error en la solicitud:', error);
       });
}

