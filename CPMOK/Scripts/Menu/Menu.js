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
   fetchData('/menu/getMenu')
      .then((data) => {
         console.log('getMenu', data)
         menus = data.Data

         tableMenu = $('#table-menu').DataTable({
            data: data.Data,
            columns: [
               {
                  data: 'MENU_PID',
                  mRender: function (data, type, row) {
                     return `
                   <div class="d-flex gap-2">
                    <button class="btn btn-danger" onclick="handleDeleteMenu('${row.MENU_PID}')">
                         <i class="fa-solid fa-trash text-light"></i>
                    </button>
                    <button class="btn btn-warning" onclick="handleModeEditMenu('${row.MENU_PID}')">
                         <i class="fa-solid fa-edit text-light"></i>
                    </button>
                   </div>
                    `
                  },
               },
               { data: 'MENU_DESC' },
               {
                  data: 'MENU_LINK',
                  mRender: function (data) {
                     return `<p>${data.substring(
                        0,
                        30
                     )}...</p> <span> <a target="_blank" href="${data}">open link</a> </span>`
                  },
               },
               {
                  data: 'MENU_ICON',
                  mRender: function (data) {
                     return `<img style="cursor: pointer" onclick="previewImage('${data}')" src="${data}" width="40" height="40""></img>`
                  },
               },
               {
                  data: 'DISTRICT',
               },
               {
                  data: 'IS_DEFAULT',
                  mRender: function (data) {
                     if (data == true) {
                        return `<div>
                        <i class="fa-solid fa-circle-check text-success"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     } else {
                        return `<div>
                        <i class="fa-solid fa-circle-xmark text-danger"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     }
                  },
               },
               {
                  data: 'IS_SSO',
                  mRender: function (data) {
                     if (data == true) {
                        return `<div>
                        <i class="fa-solid fa-circle-check text-success"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     } else {
                        return `<div>
                        <i class="fa-solid fa-circle-xmark text-danger"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     }
                  },
               },
               {
                  data: 'IS_AVAILABLE',
                  mRender: function (data) {
                     if (data == true) {
                        return `<div>
                        <i class="fa-solid fa-circle-check text-success"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     } else {
                        return `<div>
                        <i class="fa-solid fa-circle-xmark text-danger"></i>
                        <span style="opacity: 0">${data}</span>
                        </div>`
                     }
                  },
               },
               {
                  data: 'DEPT_GROUP_NAME',
               },
               {
                  data: 'ACL_GROUP_NAME',
               },
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
   $(`#btn-submit-menu`).html(
      `<img src="/content/img/loadingAnimate.svg" alt="" width="20px">`
   )

   const id = document.getElementById('id-menu').value
   const menu = document.getElementById('name-menu').value
   const generalAppLink = document.getElementById('general-app-link').value
   const generalAppID = document.getElementById('general-app-id').value
   const iosAppLink = document.getElementById('ios-app-link').value
   const iosAppId = document.getElementById('ios-app-id').value
   let iconFile = ''
   const district = document.getElementById('district').value
   const isDefault = document.getElementById('default').checked
   const sso = document.getElementById('sso').checked
   const available = document.getElementById('available').checked
   const dept_group = document.getElementById('dept_group').value
   const acl_group = document.getElementById('acl_group').value
   const downloadUrl = document.getElementById('downloadUrl').value

   if (!validateField(menu, 'menu')) {
      $(`#btn-submit-menu`).html(`Submit`)
      return false
   }
   if (!validateField(generalAppLink, 'general app link')) {
      $(`#btn-submit-menu`).html(`Submit`)
      return false
   }
   if (!validateField(district, 'district')) {
      $(`#btn-submit-menu`).html(`Submit`)
      return false
   }

   iconFile = await convertToBase64('icon-file')
   const indexStart = iconFile.indexOf(',')

   iconFile = iconFile.substring(indexStart + 1)
   const data = {
      MENU_PID: id,
      MENU_DESC: menu,
      MENU_LINK: generalAppLink,
      MENU_LINK_IOS: iosAppLink,
      MENU_ICON: iconFile,
      DISTRICT: district,
      IS_DEFAULT: isDefault,
      IS_SSO: sso,
      androidAppId: generalAppID,
      iosAppId,
      downloadUrl: downloadUrl,
      GROUP_ID: acl_group,
      GROUPING: dept_group,
      IS_AVAILABLE: available,
   }

   // return false
   if (isModeEditMenu) {
      // logic edit
      postData('/menu/updateMenu', data)
         .then((responseData) => {
            if (responseData.Status == true) {
               console.log('responseData', responseData)

               Toast.fire({
                  icon: 'success',
                  title: responseData.Message,
               })

               menus = menus.map((item) => {
                  if (item.MENU_PID == data.MENU_PID) {
                     console.log('menu', data)
                     item.MENU_DESC = responseData.Data.MENU_DESC
                     item.MENU_LINK = responseData.Data.MENU_LINK
                     item.MENU_LINK_IOS = responseData.Data.MENU_LINK_IOS
                     item.MENU_ICON = responseData.Data.MENU_ICON
                     item.DISTRICT = responseData.Data.DISTRICT
                     item.IS_DEFAULT = responseData.Data.IS_DEFAULT
                     item.IS_SSO = responseData.Data.IS_SSO
                     item.androidAppId = responseData.Data.androidAppId
                     item.iosAppId = responseData.Data.iosAppId
                     item.downloadUrl = responseData.Data.downloadUrl
                     item.GROUP_ID = responseData.Data.GROUP_ID
                     item.GROUPING = responseData.Data.GROUPING
                     item.IS_AVAILABLE = responseData.Data.IS_AVAILABLE
                     item.ACL_GROUP_NAME = responseData.Data.ACL_GROUP_NAME
                     item.DEPT_GROUP_NAME = responseData.Data.DEPT_GROUP_NAME
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
      // logic insert
      postData('/menu/createMenu', data)
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
            MENU_PID: id,
         }

         postData('/menu/deleteMenu', data)
            .then((response) => {
               if (response.Status === true) {
                  menus = menus.filter((obj) => {
                     if (obj.MENU_PID == id) {
                        return false
                     }

                     return true
                  })

                  tableMenu.clear().draw()
                  tableMenu.rows.add(menus).draw()

                  Swal.fire('Deleted!', 'Menu Berhasil Dihapus!', 'success')
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
   const menu = menus.find((item) => item.MENU_PID == id)
   console.log('menu', menu)

   $('#btn-cancel-menu').show()

   isModeEditMenu = true

   $('#id-menu').val(id)
   $('#name-menu').val(menu.MENU_DESC)
   $('#general-app-link').val(menu.MENU_LINK)
   $('#general-app-id').val(menu.androidAppId)
   $('#ios-app-link').val(menu.MENU_LINK_IOS)
   $('#ios-app-id').val(menu.iosAppId)
   $('#downloadUrl').val(menu.downloadUrl)

   $('#default').prop('checked', menu.IS_DEFAULT)
   $('#sso').prop('checked', menu.IS_SSO)
   $('#available').prop('checked', menu.IS_AVAILABLE)

   $('#district').val(menu.DISTRICT).trigger('change')
   $('#default').val(menu.IS_DEFAULT).trigger('change')
   $('#dept_group').val(menu.GROUPING).trigger('change')
   $('#acl_group').val(menu.GROUP_ID).trigger('change')
}

const handleCancelEditMenu = () => {
   isModeEditMenu = false

   $('#id-menu').val('')
   $('#name-menu').val('')
   $('#general-app-link').val('')
   $('#general-app-id').val('')
   $('#ios-app-link').val('')
   $('#ios-app-id').val('')
   $('#downloadUrl').val('')

   $('#default').prop('checked', false)
   $('#sso').prop('checked', false)
   $('#available').prop('checked', false)

   $('#district').val('').trigger('change')
   $('#default').val('').trigger('change')
   $('#dept_group').val('').trigger('change')
   $('#acl_group').val('').trigger('change')

   $('#btn-cancel-menu').hide()
}

const previewImage = (data) => {
   $('#preview-image').attr('src', data)
   $('#modalPreviewImage').modal('show')
}
