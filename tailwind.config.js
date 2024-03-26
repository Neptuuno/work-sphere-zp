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
                'primary-dark': '#59c240',
            },
            boxShadow: {
                'offset': '4px 4px 0px 0px #000000',
            }
        },
    },
    plugins: [],
    safelist: [
      {
        pattern: /bg-(gray|red|yellow|green|blue|indigo|purple|pink)-(100|200|300|400|500|600|700|800|900)/,
      }
    ]
}

