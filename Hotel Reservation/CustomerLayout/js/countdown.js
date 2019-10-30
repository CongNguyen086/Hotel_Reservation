/**
 * Created by Dell on 11/08/2017.
 */
function countdown() {
    var now= new Date();
    var enventDate= new Date(2018,1, 1);
    var currentTime= now.getTime();
    var eventTime = enventDate.getTime();
    var remTime = eventTime -currentTime;
    var s= Math.floor(remTime/ 1000);
    var m=Math.floor(s/60);
    var h=Math.floor(m/60);
    var d= Math.floor(h/24);
    h %=24;
    m %=60;
    s %=60;
    h=(h<10)? "0" +h:h;
    m=(m<10)? "0" +m:m;
    s=(s<10)? "0" +s:s;
    document.getElementById("days").textContent=d;
    // document.getElementById("days").textContent=d;
    document.getElementById("days").innerText=d;

    document.getElementById("hour").textContent=h;
    document.getElementById("minutes").textContent=m;
    document.getElementById("second").textContent=s;
    setTimeout(countdown, 1000);
}
countdown();