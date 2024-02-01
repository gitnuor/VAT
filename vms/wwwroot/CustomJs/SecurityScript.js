document.addEventListener('contextmenu', function (e) {
    e.preventDefault();
    alert("Due to security reason, This event is disabled.");
});
document.onkeydown = function (e) {
    var keyCode = e.which || e.keyCode;
    if (keyCode == 123) {
        alert("Due to security reason, This event is disabled.");
        return false;
    }
    if (e.ctrlKey && e.shiftKey && keyCode == 'I'.charCodeAt(0)) {
        alert("Due to security reason, This event is disabled.");
        return false;
    }
    if (e.ctrlKey && e.shiftKey && keyCode == 'C'.charCodeAt(0)) {
        alert("Due to security reason, This event is disabled.");
        return false;
    }
    if (e.ctrlKey && e.shiftKey && keyCode == 'J'.charCodeAt(0)) {
        alert("Due to security reason, This event is disabled.");
        return false;
    }
    if (e.ctrlKey && keyCode == 'U'.charCodeAt(0)) {
        alert("Due to security reason, This event is disabled.");
        return false;
    }
}