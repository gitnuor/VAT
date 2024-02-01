Array.from(document.getElementsByClassName('hs-code')).forEach(elementInfo => {
    IMask(elementInfo, {
        mask: '0000.00.00',
        lazy: false
    });
});