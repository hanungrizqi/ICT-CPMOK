let menus = null
let tableMenu = null
let isModeEditMenu = false

$(document).ready(function () {
    $('#btn-cancel-menu').hide()

    fetchData('/master/GetACLGroup')
        .then((data) => {
            let options = `
      <option selected value="">select acl group</option> 
      `

            data.Data.forEach((item) => {
                options += `<option value="${item.GROUP_ID}">${item.GROUP_NAME}</option>`
            })

            $('#acl_group').html(options)
        })
        .catch((error) => {
            console.error(error)
            Toast.fire({
                icon: 'error',
                title: 'Error get data acl group: ' + error.message,
            })
        })

    fetchData('/master/GetDeptGroup')
        .then((data) => {
            let options = `
      <option selected value="">select dept group</option> 
      `
            data.Data.forEach((item) => {
                options += `<option value="${item.ID_GROUP}">${item.NAMA_GROUP}</option>`
            })

            $('#dept_group').html(options)
        })
        .catch((error) => {
            console.error(error)
            Toast.fire({
                icon: 'error',
                title: 'Error get data dept group: ' + error.message,
            })
        })

    fetchData('/master/getDistrict')
        .then((data) => {
            let districtOptions = `
         <option selected value="">select district</option> 
         `

            data.Data.forEach((district) => {
                districtOptions += `<option value="${district.DSTRCT_CODE}">${district.DSTRCT_CODE}</option>`
            })
            $('.get-list-district').html(districtOptions)
        })
        .catch((error) => {
            console.error(error)
            Toast.fire({
                icon: 'error',
                title: 'Error get data district: ' + error.message,
            })
        })

    // manage menu
    fetchData('/appversion/GetVersion')
        .then((data) => {
            console.log('getMenu', data)
            menus = data.Data

            tableMenu = $('#table-menu').DataTable({
                data: data.Data,
                columns: [
                    {
                        data: 'os_name',
                        mRender: function (data, type, row) {
                            return `
                   <div class="d-flex gap-2">
                    <button class="btn btn-danger" onclick="handleDeleteMenu('${row.os_name}')">
                         <i class="fa-solid fa-trash text-light"></i>
                    </button>
                    <button class="btn btn-warning" onclick="handleModeEditMenu('${row.os_name}')">
                         <i class="fa-solid fa-edit text-light"></i>
                    </button>
                   </div>
                    `
                        },
                    },
                    { data: 'os_name' },
                    { data: 'app_version' },
                    {
                        data: 'release_date',
                        render: function (data, type, row) {
                            const tanggal = moment(data).format("DD MMM YYYY HH:mm");
                            return tanggal;
                        }
                    }
                ],
            })
        })
        .catch((error) => {
            console.error(error)
            Toast.fire({
                icon: 'error',
                title: 'Error get data menu: ' + error.message,
            })
        })
})

const handleSubmitMenu = async () => {
    debugger
    $(`#btn-submit-menu`).html(
        `<img src="/content/img/loadingAnimate.svg" alt="" width="20px">`
    )

    const id = document.getElementById('id-version').value
    const os_name = document.getElementById('operating-system').value
    const app_version = document.getElementById('version').value
    const release_date = document.getElementById('release-date').value

    const data = {
        id: id,
        os_name: os_name,
        app_version: app_version,
        release_date: release_date,
    }

    // return false
    if (isModeEditMenu) {
        // logic edit
        postData('/appversion/updateVersion', data)
            .then((responseData) => {
                if (responseData.Status == true) {
                    console.log('responseData', responseData)
                    
                    Toast.fire({
                        icon: 'success',
                        title: responseData.Message,
                    })

                    menus = menus.map((item) => {
                        if (item.os_name == data.os_name) {
                            console.log('menu', data)
                            item.app_version = responseData.Data.app_version
                            item.release_date = responseData.Data.release_date
                            return item
                        }
                        return item
                    })

                    tableMenu.clear().draw()
                    tableMenu.rows.add(menus).draw()

                    $(`#btn-submit-menu`).html(`Simpan`)

                    handleCancelEditMenu()
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: responseData.Message,
                    })

                    $(`#btn-submit-menu`).html(`Simpan`)
                    handleCancelEditMenu()
                }
            })
            .catch((error) => {
                // Handle any errors that occur during the POST request
                console.error(error)

                Toast.fire({
                    icon: 'error',
                    title: error.message,
                })

                $(`#btn-submit-menu`).html(`Simpan`)
            })
    } else {
        debugger
        // logic insert
        postData('/appversion/createVersion', data)
            .then((responseData) => {
                if (responseData.Status == true) {
                    Toast.fire({
                        icon: 'success',
                        title: responseData.Message,
                    })

                    menus.unshift(responseData.Data)

                    tableMenu.clear().draw()
                    tableMenu.rows.add(menus).draw()

                    $('#go-select-tahun').val('').trigger('change')
                    $('#go-select-district').val('').trigger('change')
                    $('#go-select-detachment').val('')
                    $('#name-group-operator').val('')
                    $('#go-select-pola-kerja').val('').trigger('change')

                    $(`#btn-submit-menu`).html(`Simpan`)
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: responseData.Message,
                    })

                    $(`#btn-submit-menu`).html(`Simpan`)
                }
            })
            .catch((error) => {
                Toast.fire({
                    icon: 'error',
                    title: error.message,
                })

                $(`#btn-submit-menu`).html(`Simpan`)
            })
    }
}

const handleDeleteMenu = (id) => {
    Swal.fire({
        title: 'Apakah kamu yakin?',
        text: 'Data akan dihapus permanent!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yaa, Hapus!',
    }).then((result) => {
        if (result.isConfirmed) {
            const data = {
                os_name: id,
            }

            postData('/appversion/DeleteVersion', data)
                .then((response) => {
                    if (response.Status === true) {
                        menus = menus.filter((obj) => {
                            if (obj.os_name == id) {
                                return false
                            }

                            return true
                        })

                        tableMenu.clear().draw()
                        tableMenu.rows.add(menus).draw()

                        Swal.fire('Deleted!', 'App Version Berhasil Dihapus!', 'success')
                    } else {
                        Toast.fire({
                            icon: 'error',
                            title: response.Message,
                        })
                    }
                })
                .catch((error) => {
                    console.error(error)

                    Toast.fire({
                        icon: 'error',
                        title: error.message,
                    })
                })
        }
    })
}

const handleModeEditMenu = (id) => {
    debugger
    const menu = menus.find((item) => item.os_name == id)
    
    $('#btn-cancel-menu').show()

    isModeEditMenu = true

    $('#id-version').val(id)
    $('#operating-system').val(menu.os_name).prop('readonly', true);
    $('#version').val(menu.app_version)
    $('#release-date').val(menu.release_date)

}

const handleCancelEditMenu = () => {
    isModeEditMenu = false
    $('#operating-system').prop('readonly', false);
    $('#id-version').val('')
    $('#operating-system').val('')
    $('#version').val('')
    $('#release-date').val('')

    $('#btn-cancel-menu').hide()
}

const previewImage = (data) => {
    $('#preview-image').attr('src', data)
    $('#modalPreviewImage').modal('show')
}
