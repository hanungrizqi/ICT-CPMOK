let announcements = null
let tableannouncement = null
let isModeEditannouncement = false

$(document).ready(function () {
   $('#btn-cancel-announcement').hide()

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

   // manage announcement
   fetchData('/announcement/getannouncement')
      .then((data) => {
         console.log('getannouncement', data)
         announcements = data.Data

         tableannouncement = $('#table-announcement').DataTable({
            data: data.Data,
            columns: [
               {
                  data: 'id',
                  mRender: function (data, type, row) {
                     return `
                   <div class="d-flex gap-2">
                    <button class="btn btn-danger" onclick="handleDeleteannouncement('${row.id}')">
                         <i class="fa-solid fa-trash text-light"></i>
                    </button>
                    <button class="btn btn-warning" onclick="handleModeEditannouncement('${row.id}')">
                         <i class="fa-solid fa-edit text-light"></i>
                    </button>
                   </div>
                    `
                  },
               },
               { data: 'title' },
               { data: 'body' },
               { data: 'order' },
               {
                  data: 'imageUrl',
                  mRender: function (data) {
                     return `<img style="cursor: pointer" onclick="previewImage('${data}')" src="${data}" width="40" height="40""></img>`
                  },
               },
               {
                  data: 'targetUrl',
                  mRender: function (data) {
                     return `<a target="_blank" href="${data}">${data}</a>`
                  },
               },
               { data: 'type' },
               { data: 'status' },
               { data: 'location' },
            ],
         })
      })
      .catch((error) => {
         console.error(error)
         Toast.fire({
            icon: 'error',
            title: 'Error get data announcement: ' + error.message,
         })
      })
})

const handleSubmitannouncement = async () => {
   $(`#btn-submit-announcement`).html(
      `<img src="/content/img/loadingAnimate.svg" alt="" width="20px">`
   )

   const id = document.getElementById('id-announcement').value
   const title = document.getElementById('title-announcement').value
   const body = document.getElementById('body-announcement').value
   const order = document.getElementById('order-announcement').value
   const type = document.getElementById('type-announcement').value
   const targetURL = document.getElementById('target-url-announcement').value
   let announcementFile = ''
   const status = document.getElementById('status-announcement').value
   const location = document.getElementById('district').value

   if (!validateField(title, 'title')) {
      $(`#btn-submit-announcement`).html(`Submit`)
      return false
   }
   if (!validateField(body, 'body')) {
      $(`#btn-submit-announcement`).html(`Submit`)
      return false
   }
   if (!validateField(order, 'order')) {
      $(`#btn-submit-announcement`).html(`Submit`)
      return false
   }
   if (!validateField(location, 'location')) {
      $(`#btn-submit-announcement`).html(`Submit`)
      return false
   }

   announcementFile = await convertToBase64('announcement-file')
   const indexStart = announcementFile.indexOf(',')

   announcementFile = announcementFile.substring(indexStart + 1)

   const data = {
      title,
      body,
      order,
      type,
      targetURL,
      status,
      location,
   }

   console.log('data', data)
   return false

   // return false
   if (isModeEditannouncement) {
      // logic edit
      postData('/announcement/updateannouncement', data)
         .then((responseData) => {
            if (responseData.Status == true) {
               console.log('responseData', responseData)

               Toast.fire({
                  icon: 'success',
                  title: responseData.Message,
               })

               announcements = announcements.map((item) => {
                  if (item.announcement_PID == data.announcement_PID) {
                     console.log('announcement', data)
                     item.announcement_DESC =
                        responseData.Data.announcement_DESC
                     item.announcement_LINK =
                        responseData.Data.announcement_LINK
                     item.announcement_LINK_IOS =
                        responseData.Data.announcement_LINK_IOS
                     item.announcement_ICON =
                        responseData.Data.announcement_ICON
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

               tableannouncement.clear().draw()
               tableannouncement.rows.add(announcements).draw()

               $(`#btn-submit-announcement`).html(`Simpan`)

               handleCancelEditannouncement()
            } else {
               Toast.fire({
                  icon: 'error',
                  title: responseData.Message,
               })

               $(`#btn-submit-announcement`).html(`Simpan`)
            }
         })
         .catch((error) => {
            // Handle any errors that occur during the POST request
            console.error(error)

            Toast.fire({
               icon: 'error',
               title: error.message,
            })

            $(`#btn-submit-announcement`).html(`Simpan`)
         })
   } else {
      // logic insert
      postData('/announcement/createannouncement', data)
         .then((responseData) => {
            if (responseData.Status == true) {
               Toast.fire({
                  icon: 'success',
                  title: responseData.Message,
               })

               announcements.unshift(responseData.Data)

               tableannouncement.clear().draw()
               tableannouncement.rows.add(announcements).draw()

               $('#go-select-tahun').val('').trigger('change')
               $('#go-select-district').val('').trigger('change')
               $('#go-select-detachment').val('')
               $('#name-group-operator').val('')
               $('#go-select-pola-kerja').val('').trigger('change')

               $(`#btn-submit-announcement`).html(`Simpan`)
            } else {
               Toast.fire({
                  icon: 'error',
                  title: responseData.Message,
               })

               $(`#btn-submit-announcement`).html(`Simpan`)
            }
         })
         .catch((error) => {
            Toast.fire({
               icon: 'error',
               title: error.message,
            })

            $(`#btn-submit-announcement`).html(`Simpan`)
         })
   }
}

const handleDeleteannouncement = (id) => {
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
            announcement_PID: id,
         }

         postData('/announcement/deleteannouncement', data)
            .then((response) => {
               if (response.Status === true) {
                  announcements = announcements.filter((obj) => {
                     if (obj.announcement_PID == id) {
                        return false
                     }

                     return true
                  })

                  tableannouncement.clear().draw()
                  tableannouncement.rows.add(announcements).draw()

                  Swal.fire(
                     'Deleted!',
                     'announcement Berhasil Dihapus!',
                     'success'
                  )
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

const handleModeEditannouncement = (id) => {
   const announcement = announcements.find(
      (item) => item.announcement_PID == id
   )
   console.log('announcement', announcement)

   $('#btn-cancel-announcement').show()

   isModeEditannouncement = true

   $('#id-announcement').val(id)
   $('#name-announcement').val(announcement.announcement_DESC)
   $('#general-app-link').val(announcement.announcement_LINK)
   $('#general-app-id').val(announcement.androidAppId)
   $('#ios-app-link').val(announcement.announcement_LINK_IOS)
   $('#ios-app-id').val(announcement.iosAppId)
   $('#downloadUrl').val(announcement.downloadUrl)

   $('#default').prop('checked', announcement.IS_DEFAULT)
   $('#sso').prop('checked', announcement.IS_SSO)
   $('#available').prop('checked', announcement.IS_AVAILABLE)

   $('#district').val(announcement.DISTRICT).trigger('change')
   $('#default').val(announcement.IS_DEFAULT).trigger('change')
   $('#dept_group').val(announcement.GROUPING).trigger('change')
   $('#acl_group').val(announcement.GROUP_ID).trigger('change')
}

const handleCancelEditannouncement = () => {
   isModeEditannouncement = false

   $('#id-announcement').val('')
   $('#name-announcement').val('')
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

   $('#btn-cancel-announcement').hide()
}

const previewImage = (data) => {
   $('#preview-image').attr('src', data)
   $('#modalPreviewImage').modal('show')
}
