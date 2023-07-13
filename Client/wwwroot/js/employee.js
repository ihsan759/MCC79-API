$(document).ready(function () {
    $('#employee').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copyHtml5',
                className: 'button-color',
                exportOptions: {
                    columns: ':visible'
                },
                split: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'pdfHtml5',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'print',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                ]
            },
            {
                extend: 'colvis',
                className: 'button-colvis-custom',
                text: 'Column'
            }
        ],
        /*columns: [
            {
                data: null,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            { data: "nik" },
            {
                data: null,
                render: (data, type, row) => {
                    return `${row.firstName} ${row.lastName}`;
                }
            },
            {
                data: "gender",
                render: (data) => {
                   return data == 1 ? 'Laki-laki' : 'Perempuan';
                }
            },
            {
                data: "birthDate",
                render: (data) => {
                    return moment(data).format("DD MMMM YYYY");
                }
            },
            {
                data: "email"
            },
            {
                data: "guid",
                render: (data) => {
                    return `<button onclick="Delete('${data}')" class='btn btn-danger'>Delete</button>
                            <button onclick="Edit('${data}')" class='btn btn-warning' data-bs-toggle="modal" data-bs-target="#edit">Edit</button>`;
                }
            }
        ],*/
    });
});

function Edit(guid) {
    $.ajax({
        url: `https://localhost:7103/Api/employees/${guid}`,
        type: "GET",
        success: (data) => {
            let nik = parseInt(data.data.nik);
            let hiringDate = moment(data.data.hiringDate).format("yyyy-MM-DD");
            let birthDate = moment(data.data.birthDate).format("yyyy-MM-DD");
            $("#enik").val(nik);
            $("#efirstName").val(data.data.firstName);
            $("#elastName").val(data.data.lastName);
            $("#ehiringDate").val(hiringDate);
            $("#ebirthDate").val(birthDate);
            $("#eemail").val(data.data.email);
            $("#ephone").val(data.data.phoneNumber);
            $("#guid").val(data.data.guid);
            $("input[name='egender']").filter(function() {
                return $(this).val() == data.data.gender;
            }).prop("checked", true);
        },
        error: (data) => {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
            })
        }
    });
}

function Update() {
    var obj = new Object();
    obj.nik = $("#enik").val().toString();
    obj.firstName = $("#efirstName").val();
    obj.lastName = $("#elastName").val();
    obj.birthDate = $("#ebirthDate").val();
    obj.gender = parseInt($("input[name='egender']:checked").val());
    obj.hiringDate = $("#ehiringDate").val();
    obj.email = $("#eemail").val();
    obj.phoneNumber = $("#ephone").val().toString();
    obj.guid = $('#guid').val();
    console.log(obj);

    $.ajax({
        url: (`https://localhost:7103/Api/employees`),
        type: "PUT",
        data: JSON.stringify(obj), // Mengubah objek menjadi string JSON
        contentType: "application/json",
        success: () => {
            Swal.fire({
                title: 'Updated!',
                text: 'Your file has been updated.',
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'ok'
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
            })
        },
        error: (xhr, status, error) => {
            let errorMessage;

            try {
                let responseJson = JSON.parse(xhr.responseText);

                // Mengakses pesan error berdasarkan format pertama
                if (responseJson.errors && typeof responseJson.errors === "object") {
                    let errors = responseJson.errors;
                    errorMessage = "";

                    for (var key in errors) {
                        if (errors.hasOwnProperty(key)) {
                            var fieldErrors = errors[key];
                            errorMessage += fieldErrors + "<br> ";
                        }
                    }

                    Swal.fire(
                        'Failed!',
                        'Failed to update data.',
                        'error'
                    );
                }

                // Mengakses pesan error berdasarkan format kedua
                if (responseJson.errors && typeof responseJson.errors === "string") {
                    errorMessage = responseJson.errors;
                }
                $("#myAlert").removeClass("alert-success").addClass("alert-danger").html(errorMessage + `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`).show();
            } catch (e) {
                // Tangani jika parsing JSON gagal
                Swal.fire({
                    title: 'Oops...',
                    text: 'Error server.',
                    icon: 'error',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'ok'
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload();
                    }
                })
            }

            // Menampilkan pesan error ke pengguna
            console.log(errorMessage);
        }
    });
}

function Delete(guid) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `https://localhost:7103/Api/employees?guid=${guid}`,
                type: "DELETE",
                success: (data) => {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'Your file has been deleted.',
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'ok'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.reload();
                        }
                    })
                },
                error: (data) => {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Failed to delete data!',
                    })
                }
            });
        }
    });
}

function Insert() {
    var obj = new Object();
    obj.nik = $("#nik").val().toString();
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.birthDate = $("#birthDate").val();
    obj.gender = parseInt($("input[name='gender']:checked").val());
    obj.hiringDate = $("#hiringDate").val();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phone").val().toString();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: ("https://localhost:7103/Api/employees"),
        type: "POST",
        data: JSON.stringify(obj), // Mengubah objek menjadi string JSON
        contentType: "application/json",
        success: () => {
            Swal.fire({
                title: 'Inserted!',
                text: 'Your file has been inserted.',
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'ok'
            }).then((result) => {
                if (result.isConfirmed) {
                    location.reload();
                }
            })
            //setTimeout(function () {
           // }, 3000);
        },
        error: (xhr, status, error) => {
            let errorMessage;

            try {
                let responseJson = JSON.parse(xhr.responseText);

                // Mengakses pesan error berdasarkan format pertama
                if (responseJson.errors && typeof responseJson.errors === "object") {
                    let errors = responseJson.errors;
                    errorMessage = "";

                    for (var key in errors) {
                        if (errors.hasOwnProperty(key)) {
                            var fieldErrors = errors[key];
                            errorMessage += fieldErrors + "<br> ";
                        }
                    }

                    Swal.fire(
                        'Failed!',
                        'Failed to insert data.',
                        'error'
                    );
                }

                // Mengakses pesan error berdasarkan format kedua
                if (responseJson.errors && typeof responseJson.errors === "string") {
                    errorMessage = responseJson.errors;
                }
                $("#myAlert").removeClass("alert-success").addClass("alert-danger").html(errorMessage + `<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>`).show();
            } catch (e) {
                // Tangani jika parsing JSON gagal
                Swal.fire({
                    title: 'Oops...',
                    text: 'Error server.',
                    icon: 'error',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'ok'
                }).then((result) => {
                    if (result.isConfirmed) {
                        location.reload();
                    }
                })
            }

            // Menampilkan pesan error ke pengguna
            console.log(errorMessage);
        }
    });
}

$(document).ready(function () {
    $("#employeeInsert").submit(function (event) {
        event.preventDefault();
        Insert();
        // Tutup modal
        $("#create").modal("hide");
    });

    $("#employeeUpdate").submit(function (event) {
        event.preventDefault();
        Update();
        // Tutup modal
        $("#edit").modal("hide");
    });
});