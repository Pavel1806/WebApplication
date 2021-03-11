
function myFunction() {
    
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[3];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

$(function () {  
      const str = document.querySelectorAll('table tr');
      const table = document.querySelector('table');
      const input = document.querySelector('.edit');
      {
        str.forEach(item => {
            item.addEventListener('click', (e) => {
                str.forEach(el => { el.classList.remove('selected'); });
                item.classList.add('selected')
            })
        })

          input.addEventListener('click', (evt) => {
              const values = [];
              table.querySelectorAll('tr').forEach((tr) => {
                  if (tr.classList.contains('selected')) {
                      values.push(tr.children[1].textContent.trim());
                  }
              });
              sendToServer(values);
          });
          //var r = window.location.href;
          function sendToServer(values) {
              console.log(values[0]);
              var body = "email=" + values[0];
              document.location = "/home/edit?" + body;
          }
      }

  });

$(function () {
    const str = document.querySelectorAll('table tr');
    const table = document.querySelector('table');
    const input = document.querySelector('.delete');
    {
        str.forEach(item => {
            item.addEventListener('click', (e) => {
                str.forEach(el => { el.classList.remove('selected'); });
                item.classList.add('selected')
            })
        })

        input.addEventListener('click', (evt) => {
            const values = [];
            table.querySelectorAll('tr').forEach((tr) => {
                if (tr.classList.contains('selected')) {
                    values.push(tr.children[1].textContent.trim());
                }
            });
            sendToServer(values);
        });
        //var r = window.location.href;
        function sendToServer(values) {
            console.log(values[0]);
            var body = "email=" + values[0];
            document.location = "/home/delete?" + body;
        }
    }

});

$(document).ready(function() {

    function addRemoveClass(theRows) {
    }
    var rows = $("table#myTable tr:not(:first-child)");
    addRemoveClass(rows);

    $(".selectDepartment").on("change", function() {
        var selected = this.value;
        if (selected != "All") {
            rows.filter("[position=" + selected + "]").show();
            rows.not("[position=" + selected + "]").hide();
            var visibleRows = rows.filter("[position=" + selected + "]");
            addRemoveClass(visibleRows);
        } else {
            rows.show();
            addRemoveClass(rows);
        }
    });
});