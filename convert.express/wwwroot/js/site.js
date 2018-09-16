// Write your JavaScript code.
window.onload = function () {
    var e = document.getElementById('Query');
    e.oninput = loadData;
    e.onpropertychange = e.oninput; // for IE8
    // e.onchange = e.oninput; // FF needs this in <select><option>...
    // other things for onload()
}



function loadData(){
    var query = document.getElementById('Query').value; 
    var url = '/api/converter?q=' + query; 


 

}

/*

   fetch(url).then(function(data) {
        console.log(data);
        var result = document.getElementById('data');
        result.innerHtml = data;
    }).catch(function(error) {
        console.log(error);
    });
*/