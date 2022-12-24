let gridApiDates;
let lPatients = [];
let lDoctors = [];
let lAllDates = [];
let lUsers = [];
let lRoles = [];
let lAccess = [];
var Int_IdDate = 0;
var Int_IdPatient = 0;
var today = "";
//get all patients and doctors
function fgetPxDx() {
    $.ajax({
        type: 'POST',
        url: url_controllers._getPxDx,
        success: function (result) {
            if (result.Success) {
                lPatients = result.lPatients;
                lDoctors = result.lDoctors;
                fGetAllDates();
                f_fillDoctorSelect();
            } else {
                fswalAlert("warning", "Ha ocurrido un error consultando los pacientes y doctores.");
                console.log(result.message);
            }
        },
        fail: function () {
            fswalAlert("error", "Ha ocurrido un error consultando los pacientes y doctores.");
        }
    });
}
//get all users
function fGetUsers() {
    $.ajax({
        type: 'POST',
        url: url_controllers._getUsers,
        success: function (result) {
            if (result.Success) {
                lUsers = result.lUsers;
                lRoles = result.lRoles;
                lAccess = result.lAccess;
                fgetPxDx();
            } else {
                fswalAlert("warning", "Ha ocurrido un error consultando los usuarios.");
                console.log(result.message);
            }
        },
        fail: function () {
            fswalAlert("error", "Ha ocurrido un error consultando los usuarios.");
        }
    });
}
//get all dates in asc order
function fGetAllDates() {
    $.ajax({
        type: 'POST',
        url: url_controllers._getAllDates,
        success: function (result) {
            if (result.Success) {
                lAllDates = result.lAllDates;
                fBuildGridDates();
            } else {
                fswalAlert("warning", "Ha ocurrido un error consultando las citas.");
                console.log(result.message);
            }
        },
        fail: function () {
            fswalAlert("error", "Ha ocurrido un error consultando las citas.");
        }
    });
}
//build ag grid with dates list
function fBuildGridDates() {
    for (var i = 0; i < lAllDates.length; i++) {
        lAllDates[i]["dateToFilter"] = reverseDate(lAllDates[i].strDate);
    }
    var columnDefs = [
        { headerName: "", cellRenderer: "buttonsDates", width: 50, editable: false, filter: false, sortable: false, lockPosition: true },
        { headerName: "Fecha cita", field: "dateToFilter", filter: 'agDateColumnFilter', editable: false, sortable: true, lockPosition: true, filterParams: filterParams, comparator: dateComparator },
        { headerName: "Paciente", field: "strPxName", editable: false, sortable: true, filter: true, lockPosition: true },
        { headerName: "Diagnóstico", field: "strDx", filter: 'agNumberColumnFilter', editable: false, sortable: true, lockPosition: true },
        { headerName: "Doctor", field: "strDxName", editable: false, sortable: true, filter: true, lockPosition: true },
    ];
    var gridOptions = {
        columnDefs: columnDefs,
        suppressRowClickSelection: true,
        defaultColDef: {
            resizable: true
        },
        components: {
            buttonsDates: buttonsDates
        },
        pagination: false,
        animateRows: true,
        suppressHorizontalScroll: false,
        rowData: lAllDates
    };
    var gridDiv = document.getElementById("agGridDates");
    new agGrid.Grid(gridDiv, gridOptions);
    gridOptions.api.setDomLayout('autoHeight');
    gridApiDates = gridOptions.api;
    setTimeout(function () { gridApiDates.sizeColumnsToFit() }, 800);
}
//icons to edit or delete any date
function buttonsDates(node) {
    const div = document.createElement("div");
    const iTag = document.createElement("i");
    iTag.classList.add("fa");
    iTag.classList.add("fa-edit");
    iTag.classList.add("text-success");
    iTag.classList.add("icon");
    iTag.addEventListener("click", () => {
        //edit date
        f_newDateButton(false);
        Int_IdDate = node.data.Int_Id;
        Int_IdPatient = node.data.Fk_IdPatient;
        $("#modalAddDate").modal("show");
        $("#labelModalAddDate").text("Editando cita de: " + node.data.strPxName);
        $("#inpDate").val(node.data.strDate);
        $("#inpPxName").val(node.data.strPxName);
        $("#inpPxLastName").val(node.data.strPxLastName);
        $("#txtDX").val(node.data.strDx);
        $("#inpEmailPx").val(node.data.strDate);
        $('#sltDoctor option[value="' + node.data.Fk_IdDoctor + '"]').prop("selected", true);
        $('#sltDoctor').trigger('change.select2');
    });
    div.append(iTag);
    const iTrash = document.createElement("i");
    iTrash.classList.add("fa");
    iTrash.classList.add("fa-trash");
    iTrash.classList.add("text-danger");
    iTrash.classList.add("icon");
    iTrash.addEventListener("click", () => {
        //delete date
        Swal.fire({
            icon: 'warning',
            title: '<div class="row"><div class="col-12">Está a punto de eliminar el la cita de: ' + node.data.strPxName + '.</div><div class="col-12"><b style="color: blue;">¿Desea continuar?</b></div></div>',
            showConfirmButton: true,
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            reverseButtons: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'No'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'POST',
                    url: url_controllers._deleteDate,
                    data: { Int_Id: node.data.Int_Id },
                    success: function (result) {
                        if (result.Success) {
                            fswalAlert("success", "Cita de: " + node.data.strPxName + " eliminada.");
                            lAllDates = lAllDates.filter(x => x.Int_Id != node.data.Int_Id);
                            gridApiDates.setRowData(lAllDates);
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error. Contacte al administrador.',
                                showConfirmButton: true
                            });
                            console.log(result.message);
                        }
                    },
                    fail: function () {
                        fswalAlert("error", "Ha ocurrido un error eliminando la cita.");
                    }
                });
            }
        });
    });
    div.append(iTrash);
    div.classList.add("divButtons");
    return div;
}
//open add date modal
function f_newDateButton(isNew) {
    if (isNew) {
        Int_IdDate = 0;
        Int_IdPatient = 0;
        $("#labelModalAddDate").text("Agendar nueva cita");
    }
    $("#inpDate").val(today);
    $("#inpPxName").val("");
    $("#inpPxLastName").val("");
    $("#txtDX").val("");
    $("#inpEmailPx").val("");
}
//save or update date
function fSaveDate() {
    if (f_verifyDataDates()) {
        $.ajax({
            type: 'POST',
            url: url_controllers._saveDate,
            data: {
                Int_Id: Int_IdDate,
                Chr_Name: $("#inpPxName").val(),
                Chr_LastName: $("#inpPxName").val(),
                Chr_DX: $("#txtDX").val(),
                Chr_Email: $("#inpPxName").val(),
                Date_Date: $("#inpDate").val(),
                Fk_IdDoctor: $("#sltDoctor").val(),
                Fk_IdPatient: Int_IdPatient,
            },
            success: function (result) {
                if (result.Success) {
                    fswalAlert("success", "Cita guardada");
                    $("#modalAddDate").modal("hide");
                    //refresh grid with new values
                    lAllDates = lAllDates.filter(x => x.Int_Id != result.newDateSaved.Int_Id);
                    lAllDates.unshift(result.newDateSaved);
                    gridApiDates.setRowData(lAllDates);
                } else {
                    fswalAlert("warning", "Ha ocurrido un error guardando la cita.", result.message);
                    console.log(result.message);
                }
            },
            fail: function () {
                fswalAlert("error", "Ha ocurrido un error guardando la cita.");
            }
        });
    }
}
//if data its complete
function f_verifyDataDates() {
    if ($("#inpPxName").val() == "") {
        fswalAlert("error", "El paciente debe contar con al menos un nombre.");
        return false;
    } else if ($("#txtDX").val() == "") {
        fswalAlert("error", "El paciente debe contar con al menos un diagnóstico.");
        return false;
    } else if ($("#inpDate").val() == "") {
        fswalAlert("error", "La cita debe contar con una fecha");
        return false;
    }
    return true;
}
//fill component list with all doctors available
function f_fillDoctorSelect() {
    $('#sltDoctor').empty();
    for (var i = 0; i < lDoctors.length; i++) {
        $('#sltDoctor').append('<option value="' + lDoctors[i].Int_Id + '">' + lDoctors[i].Chr_Name + '</option>');
    }
    $('#sltDoctor').trigger('change.select2');
}
//alert messages
function fswalAlert(icon, message, html) {
    Swal.fire({
        icon: icon,
        title: message,
        html: html != undefined ? html : "",
        showConfirmButton: true
    });
}
//open modal to add new doctor
function f_newDocButton() {
    $("#inpDxName").val("");
    $("#inpDxLastName").val("");
    $("#inpCPDx").val("");
    $("#inpEmailDx").val("");
}
//save new doctor
function fSaveDoctor() {
    if (f_verifyDataDoctor()) {
        $.ajax({
            type: 'POST',
            url: url_controllers._saveDoctor,
            data: {
                Chr_Name: $("#inpDxName").val(),
                Chr_LastName: $("#inpDxLastName").val(),
                Chr_ProfessionalLicense: $("#inpCPDx").val(),
                Chr_Email: $("#inpEmailDx").val()
            },
            success: function (result) {
                if (result.Success) {
                    fswalAlert("success", "Doctor guardado");
                    $("#modalAddDoc").modal("hide");
                    lDoctors.push(result._doc);
                    //fill select component with new values
                    f_fillDoctorSelect();
                } else {
                    fswalAlert("warning", "Ha ocurrido un error guardando el doctor.");
                    console.log(result.message);
                }
            },
            fail: function () {
                fswalAlert("error", "Ha ocurrido un error guardando el doctor.");
            }
        });
    }
}
//if data doctor its complete
function f_verifyDataDoctor() {
    if ($("#inpDxName").val() == "") {
        fswalAlert("error", "El doctor debe contar con al menos un nombre.");
        return false;
    } else if ($("#inpDxLastName").val() == "") {
        fswalAlert("error", "El doctor debe contar con al menos un apellido.");
        return false;
    } else if ($("#inpCPDx").val() == "") {
        fswalAlert("error", "El Doctor debe contar con Cédula Profesional");
        return false;
    }
    return true;
}
//open modal to add new user
function f_newDocButton() {
    $("#inpNameUser").val("");
    $("#inpUserName").val("");
    $("#inpPassword1").val("");
    $("#inpPassword2").val("");
    $("#inpEmailUser").val("");
}
//save new user
function fSaveUser() {
    if (f_verifyDataUser()) {
        $.ajax({
            type: 'POST',
            url: url_controllers._saveUser,
            data: {
                Chr_Name: $("#inpNameUser").val(),
                Chr_UserName: $("#inpUserName").val(),
                Chr_Password: $("#inpPassword1").val(),
                Chr_Email: $("#inpEmailUser").val(),
            },
            success: function (result) {
                if (result.Success) {
                    fswalAlert("success", "Usuario guardado");
                    $("#modalAddUser").modal("hide");
                } else {
                    fswalAlert("warning", "Ha ocurrido un error guardando el usuario.", result.message);
                    console.log(result.message);
                }
            },
            fail: function () {
                fswalAlert("error", "Ha ocurrido un error guardando el usuario.");
            }
        });
    }
}
//if user data its complete
function f_verifyDataUser() {
    if ($("#inpNameUser").val() == "") {
        fswalAlert("error", "El usuario debe contar con al menos un nombre.");
        return false;
    } else if ($("#inpUserName").val() == "") {
        fswalAlert("error", "El doctor debe contar con al menos un username.");
        return false;
    } else if ($("#inpPassword1").val() == "") {
        fswalAlert("error", "El Doctor debe contar con una contraseña");
        return false;
    } else if ($("#inpPassword1").val() !== $("#inpPassword2").val()) {
        fswalAlert("error", "Las contraseñas no coinciden");
        return false;
    }
    return true;
}
//date from 2022-12-24 to 24/12/2022
function reverseDate(cadena) {
    if (cadena == "" || cadena == null) {
        return "";
    }
    var cadenarevertida = cadena.split("-")
    return cadenarevertida[2] + "/" + cadenarevertida[1] + "/" + cadenarevertida[0];
}
//parameters to ag grid column with dates
var filterParams = {
    comparator: function (filterLocalDateAtMidnight, cellValue) {
        var dateAsString = cellValue;
        if (dateAsString == null) return -1;
        var dateParts = dateAsString.split('/');
        var cellDate = new Date(
            Number(dateParts[2]),
            Number(dateParts[1]) - 1,
            Number(dateParts[0])
        );

        if (filterLocalDateAtMidnight.getTime() === cellDate.getTime()) {
            return 0;
        }

        if (cellDate < filterLocalDateAtMidnight) {
            return -1;
        }

        if (cellDate > filterLocalDateAtMidnight) {
            return 1;
        }
    },
    browserDatePicker: true,
    minValidYear: 2000,
};
//funciton to sort dates on ag grid
function dateComparator(date1, date2) {
    var date1Number = monthToComparableNumber(date1);
    var date2Number = monthToComparableNumber(date2);

    if (date1Number === null && date2Number === null) {
        return 0;
    }
    if (date1Number === null) {
        return -1;
    }
    if (date2Number === null) {
        return 1;
    }

    return date1Number - date2Number;
}
// eg 29/08/2004 gets converted to 20040829
function monthToComparableNumber(date) {
    if (date === undefined || date === null || date.length !== 10) {
        return null;
    }

    var yearNumber = date.substring(6, 10);
    var monthNumber = date.substring(3, 5);
    var dayNumber = date.substring(0, 2);

    var result = yearNumber * 10000 + monthNumber * 100 + dayNumber;
    return result;
}
$(document).ready(function () {
    fGetUsers();
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    today = now.getFullYear() + "-" + month + "-" + (day);
    $("#inpDate").val(today);
    $(".select2").select2();
});