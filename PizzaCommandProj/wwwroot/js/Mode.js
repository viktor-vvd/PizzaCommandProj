let count=0;
function func(){
    
    let div1 = document.getElementById("headh21");
    if (count%2==0)
        div1.classList.add("edit");
    else
        div1.classList.remove("edit");

    let div2 = document.getElementById("headh22");
    if (count % 2 == 0)
        div2.classList.add("edit");
    else
        div2.classList.remove("edit");

    let div3 = document.getElementById("adressblock");
    if (count % 2 == 0)
        div3.classList.add("edit");
    else
        div3.classList.remove("edit");

    let div4 = document.getElementById("textblock1");
    if (count % 2 == 0)
        div4.classList.add("edit");
    else
        div4.classList.remove("edit");

    let div5 = document.getElementById("textblock2");
    if (count % 2 == 0)
        div5.classList.add("edit");
    else
        div5.classList.remove("edit");

    let div6 = document.getElementById("footerblock");
    if (count % 2 == 0)
        div6.classList.add("edit");
    else
        div6.classList.remove("edit");

    if (count % 2 == 0)
        VANTA.TOPOLOGY({
            el: "#mainanim",
            mouseControls: true,
            touchControls: true,
            gyroControls: false,
            minHeight: 200.00,
            minWidth: 200.00,
            scale: 1.00,
            scaleMobile: 1.00,
            color: 0x48270c,
            backgroundColor: 0xf0d6b3
        });
    else
        VANTA.TOPOLOGY({
            el: "#mainanim",
            mouseControls: true,
            touchControls: true,
            gyroControls: false,
            minHeight: 200.00,
            minWidth: 200.00,
            scale: 1.00,
            scaleMobile: 1.00,
            color: 0x764232,
            backgroundColor: 0x251a11
        });
        
    count++;
}