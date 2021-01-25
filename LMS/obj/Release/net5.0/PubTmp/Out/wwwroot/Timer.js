let timer = null;
let time  = 60;
timer = setInterval(() =>
{
    if(time > 0)
        time = time - 1
    document.getElementById("timer").innerHTML = "<br/>" + "&nbsp; ( تا " + enNumberToPersian(time.toString()) + " ثانیه ی دیگر، شما به ( دوره های من ) هدایت خواهید شد ) " + "&nbsp; <br><br>";
    if(time === 0) clearInterval(timer);
}, 1000);

setTimeout(() =>
{
    window.location.href = "http://localhost:3001/terms/owned";
}, 60000);

/*-------------------------------------------------------------------*/

function enNumberToPersian($string)
{
    const numbers = ['۰', '١', '٢', '٣', '۴', '۵', '۶', '٧', '٨', '٩'];

    let chars = $string?.split('');
    for (let i = 0; i < chars?.length; i++)
    {
        if(/\d/.test(chars[i])) /*بررسی عدد بودن یا نبودن کاراکتر هدف*/
        {
            chars[i] = numbers[chars[i]];
        }
    }

    return chars?.join('');
}