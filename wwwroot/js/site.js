// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function displayEcho(element,json) {
    let result = `<div class="row"><div class="col">Host name</div><div class="col">${json.hostName}</div></div>`
    result += `<div class="row"><div class="col">Host IP</div><div class="col">${json.hostIp}</div></div>`
    result += `<div class="row"><div class="col">Host Computer name</div><div class="col">${json.computerName}</div></div>`
    result += `<div class="row"><div class="col">Request IP</div><div class="col">${json.remoteIp}</div>`
    result += `<div class="row"><div class="col">Processed on</div><div class="col">${json.time}</div>`
    if (json.message) { result += `<div class="row"><div class="col"></div><div class="col">${json.message}</div>` }
    result += `</div >`
    element.innerHTML = result

}
