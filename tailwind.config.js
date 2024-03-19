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
    colors: {
      'primary': '#62D745',
    },
    extend: {
      boxShadow: {
        'offset': '4px 4px 0px 0px #000000',
      }
    },
  },
  plugins: [],
}

