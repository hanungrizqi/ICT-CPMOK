let isNrp = false
let isDistrict = false
let nrpOptions = [];

$(document).ready(function () {
    debugger
    handleNotificationTypeChange();

    fetchData('/master/getDistrict')
        .then((data) => {
            let districtOptions = [{ id: '', text: 'Select District' }];

            data.Data.forEach((item) => {
                districtOptions.push({
                    id: item.DSTRCT_CODE,
                    text: `${item.DSTRCT_CODE}`
                });
            });

            $('.get-list-district').select2({
                data: districtOptions,
                placeholder: 'Select District',
                allowClear: true
            });
        })
        .catch((error) => {
            console.error(error);
            Toast.fire({
                icon: 'error',
                title: 'Error getting data: ' + error.message,
            });
        });

    fetchData('/master/GetNrp')
        .then((data) => {
            nrpOptions = [{ id: '', text: 'Select NRP' }];

            data.Data.forEach((item) => {
                nrpOptions.push({
                    id: item.EMPLOYEE_ID,
                    text: `${item.EMPLOYEE_ID} - ${item.NAME}`
                });
            });

            $('.get-list-nrp').select2({
                data: nrpOptions,
                placeholder: 'Select NRP',
                allowClear: true,
                multiple: true
            });
        })
        .catch((error) => {
            console.error(error);
            Toast.fire({
                icon: 'error',
                title: 'Error getting data: ' + error.message,
            });
        });

});

function handleNotificationTypeChange() {
    debugger;
    var selectedValue = document.getElementById('type-announcement').value;
    $('#site-row').hide();
    $('#nrp-row').hide();

    if (selectedValue === 'DISTRICT') {
        $('#site-row').show()
        isNrp = true
    } else if (selectedValue === 'NRP') {
        $('#nrp-row').show()
        isDistrict = true
    }
}

const Submit = async () => {
    debugger
    var notificationType = document.getElementById('type-announcement').value;
    var title = document.getElementById('title-notif').value;
    var body = document.getElementById('body-notif').value;
    var target = document.getElementById('target').value;
    var eventid = document.getElementById('eventid').value;

    // Additional variables for specific types
    var district = '';
    var nrp = '';

    if (notificationType === 'DISTRICT') {
        district = document.getElementById('district').value;
        isDistrict = true
    } else if (notificationType === 'NRP') {
        nrp = document.getElementById('nrp').value;
        isNrp = true
    }

    var selectedNrpValues = $('#nrp').val();
    var sData = {}
    sData = {
        title: $("#title-notif").val(),
        body: $("#body-notif").val(),
        target: $("#target").val(),
        site: district,
        event_id: $("#eventid").val(),
        nrp: selectedNrpValues,
        notifType: $("#type-announcement").val()
    }

    if (isNrp) {
        $.ajax({
            type: 'POST',
            dataType: "json",
            contentType: "application/json",
            url: '/pushnotif/x',
            data: JSON.stringify(sData),
            success: function (Status) {
                debugger;
                console.log('Form nrp submitted successfully');
                Toast.fire({
                    icon: 'success',
                    title: Status.remarks,
                });

                $("#title-notif").val('');
                $("#body-notif").val('');
                $("#target").val('');
                $("#eventid").val('');
                $("#type-announcement").val('nones').change();
                $("#district").val('').trigger('change');
                $("#nrp").val('').trigger('change');
            },
            error: function (error) {
                console.error('Error submitting form:', error);
                Toast.fire({
                    icon: 'error',
                    title: error.remarks,
                })
            }
        });
    } else if (isDistrict) {
        $.ajax({
            type: 'POST',
            dataType: "json",
            contentType: "application/json",
            url: '/pushnotif/x',
            data: JSON.stringify(sData),
            success: function (Status) {
                debugger;
                console.log('Form district submitted successfully');
                Toast.fire({
                    icon: 'success',
                    title: Status.remarks,
                });

                $("#title-notif").val('');
                $("#body-notif").val('');
                $("#target").val('');
                $("#eventid").val('');
                $("#type-announcement").val('nones').change();
                $("#district").val('').trigger('change');
                $("#nrp").val('').trigger('change');
            },
            error: function (error) {
                console.error('Error submitting form:', error);
                Toast.fire({
                    icon: 'error',
                    title: error,
                })
            }
        });
    }
    
}
