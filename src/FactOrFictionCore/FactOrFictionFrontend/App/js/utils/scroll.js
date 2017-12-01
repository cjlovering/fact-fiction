const scrollTo = (element, to, duration) => {
    const easeInOutQuad = (t, b, c, d) => {
        let tt = t;
        tt /= d/2;
        if (t < 1) return c/2*tt*tt + b;
        tt--;
        return -c/2 * (tt*(tt-2) - 1) + b;
    };

    const start = element.scrollTop;
    const change = to - start;
    let currentTime = 0;
    const increment = 5;
        
    const animateScroll = () => {        
        currentTime += increment;
        const val = easeInOutQuad(currentTime, start, change, duration);
        element.scrollTop = val;
        if(currentTime < duration) {
            setTimeout(animateScroll, increment);
        }
    };
    animateScroll();
}

 const changeFocus = (selectedEntryId, type) => {
    if (selectedEntryId == "") return;

    const paneId = type == "sentence" ? "list-view" : "result-box";
    const selectType = type == "sentence" ? "fact-card" : "sentence";
    
    const elm = document.getElementById(`${selectedEntryId}-${selectType}`);
    const containerElm =  document.getElementById(paneId);

    scrollTo(containerElm, elm.offsetTop - 150, 300); 
}

export { changeFocus };