//helpers
async function fetchData(url) {
   try {
      const response = await fetch(url)

      if (response.ok) {
         const data = await response.json() // Assuming the response is in JSON format
         return data
      } else {
         // Handle non-successful HTTP response
         throw new Error(
            `Failed to fetch data. Status code: ${response.status}`
         )
      }
   } catch (error) {
      // Handle exceptions
      throw new Error(`An error occurred while fetching data: ${error.message}`)
   }
}

async function postData(url, data) {
   return fetch(url, {
      method: 'POST',
      headers: {
         'Content-Type': 'application/json', // Adjust content type if needed
      },
      body: JSON.stringify(data),
   })
      .then((response) => {
         if (!response.ok) {
            throw new Error('Failed to POST data')
         }
         return response.json()
      })
      .catch((error) => {
         throw error
      })
}

const Toast = Swal.mixin({
   toast: true,
   position: 'top-end',
   showConfirmButton: false,
   timer: 5000,
   timerProgressBar: true,
   didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer)
      toast.addEventListener('mouseleave', Swal.resumeTimer)
   },
})

const generateYearArray = () => {
   const currentYear = new Date().getFullYear()
   const yearArray = []

   for (let i = 0; i < 3; i++) {
      const nextYear = currentYear + i
      yearArray.push(nextYear.toString())
   }

   return yearArray
}

function validateField(value, fieldName, errorMessage) {
   if (value === '') {
      Toast.fire({
         icon: 'error',
         title: `Field ${fieldName} tidak boleh kosong.`,
      })

      $(`#btn-submit-menu`).html(`Simpan`)
      return false
   }
   return true
}

function convertToBase64(inputId) {
   return new Promise((resolve, reject) => {
      var input = document.getElementById(inputId)
      var file = input.files[0]

      if (file) {
         var reader = new FileReader()

         reader.onload = function (e) {
            var base64String = e.target.result
            resolve(base64String)
         }

         reader.onerror = function (error) {
            resolve('')
         }

         reader.readAsDataURL(file)
      } else {
         resolve('')
      }
   })
}
