/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml'
  ],
  theme: {
    fontFamily: {
        'sans': ['Outfit', 'sans-serif'],
    },
    
    extend: {
      colors: {
        'primary': '#62D745',
      },
      boxShadow: {
        'offset': '4px 4px 0px 0px #000000',
      }
    },
  },
  plugins: [],
}

