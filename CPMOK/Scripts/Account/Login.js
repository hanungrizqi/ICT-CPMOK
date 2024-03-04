function handleSubmit(e) {
   {
      e.preventDefault() // Prevent the default form submission

      $(`#buttonSubmit`).html(
         `<img src="/Content/img/loadingAnimate.svg" alt="" width="20px">`
      )

      $('#buttonSubmit').prop('disabled', true)

      const data = {
         username: $('#nrp').val(),
         password: $('#password').val(),
      }

      $.ajax({
         type: 'POST',
         url: '/Account/PostLogin',
         data: data,
         success: function (response) {
            $(`#buttonSubmit`).html(`Submit`)

            $('#buttonSubmit').prop('disabled', false)

            if (response.Remarks) {
               window.location.href = '/'
            } else {
               Toast.fire({
                  icon: 'error',
                  title: 'NRP atau password salah.',
               })
            }
         },
         error: function (error) {
            $('#buttonSubmit').prop('disabled', false)

            $(`#buttonSubmit`).html(`Submit`)
            Toast.fire({
               icon: 'error',
               title: 'Ada masalah pada server.',
            })
         },
      })
   }
}

const Toast = Swal.mixin({
   toast: true,
   position: 'top-end',
   showConfirmButton: false,
   timer: 3000,
   timerProgressBar: true,
   didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer)
      toast.addEventListener('mouseleave', Swal.resumeTimer)
   },
})
