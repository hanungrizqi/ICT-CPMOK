let menus = null
let tableMenu = null
let isModeEditMenu = false

$(document).ready(function () {
    $('#btn-cancel-menu').hide()

    // manage menu
    fetchData('/userphone/GetPhone')
        .then((data) => {
            console.log('getMenu', data)
            menus = data.Data

            tableMenu = $('#table-menu').DataTable({
                data: data.Data,
                columns: [
                    {
                        data: 'nrp',
                        mRender: function (data, type, row) {
                            return `
                   <div class="d-flex gap-2">
                    <button class="btn btn-danger" onclick="handleDeleteMenu('${row.nrp}')">
                         <i class="fa-solid fa-trash text-light"></i>
                    </button>
                    <button class="btn btn-warning" onclick="handleModeEditMenu('${row.nrp}')">
                         <i class="fa-solid fa-edit text-light"></i>
                    </button>
                   </div>
                    `
                        },
                    },
                    { data: 'nrp' },
                    { data: 'phoneNo' }
                ],
            })
        })
        .catch((error) => {
            console.error(error)
            Toast.fire({
                icon: 'error',
                title: 'Error get data phone: ' + error.message,
            })
        })
})

const handleSubmitMenu = async () => {
    debugger
    $(`#btn-submit-menu`).html(
        `<img src="/content/img/loadingAnimate.svg" alt="" width="20px">`
    )

    const id = document.getElementById('id-phone').value
    const nrp = document.getElementById('nrp').value
    const phone = document.getElementById('phone').value

    const data = {
        id: id,
        nrp: nrp,
        phone: phone,
    }

    // return false
    if (isModeEditMenu) {
        // logic edit
        postData('/userphone/updatePhone', data)
            .then((responseData) => {
                if (responseData.Status == true) {
                    console.log('responseData', responseData)

                    Toast.fire({
                        icon: 'success',
                        title: responseData.Message,
                    })

                    menus = menus.map((item) => {
                        debugger
                        if (item.nrp == data.nrp) {
                            console.log('menu', data)
                            item.nrp = responseData.Data.nrp
                            item.phoneNo = responseData.Data.phoneNo
                            return item
                        }
                        return item
                    })

                    tableMenu.clear().draw()
                    tableMenu.rows.add(menus).draw()

                    $(`#btn-submit-menu`).html(`Simpan`)

                    handleCancelEditPhone()
                } else {
                    Toast.fire({
                        icon: 'error',
                        title: responseData.Message,
                    })

                    $(`#btn-submit-menu`).html(`Simpan`)
                    handleCancelEditPhone()
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
        postData('/userphone/createPhone', data)
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
                    handleCancelEditPhone()
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
                nrp: id,
            }

            postData('/userphone/DeletePhone', data)
                .then((response) => {
                    if (response.Status === true) {
                        menus = menus.filter((obj) => {
                            if (obj.nrp == id) {
                                return false
                            }

                            return true
                        })

                        tableMenu.clear().draw()
                        tableMenu.rows.add(menus).draw()

                        Swal.fire('Deleted!', 'Phone Berhasil Dihapus!', 'success')
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
    const menu = menus.find((item) => item.nrp == id)

    $('#btn-cancel-menu').show()

    isModeEditMenu = true

    $('#id-phone').val(id)
    $('#nrp').val(menu.nrp)
    $('#phone').val(menu.phoneNo)

}

const handleCancelEditPhone = () => {
    isModeEditMenu = false
    $('#id-phone').val('')
    $('#nrp').val('')
    $('#phone').val('')

    $('#btn-cancel-menu').hide()
}
