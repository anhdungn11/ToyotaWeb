function sendMessage() {
    var input = document.getElementById("chatInput");
    var message = input.value.trim();
    if (message === "") return;

    var chatBody = document.getElementById("chatBody");

    chatBody.innerHTML += `
        <div style="text-align:right;margin:5px 0;">
            <span style="background:#d71920;color:white;padding:6px 10px;border-radius:10px;">
                ${message}
            </span>
        </div>
    `;

    fetch('/Chat/Ask', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: 'message=' + encodeURIComponent(message)
    })
    .then(response => response.text())
    .then(data => {
        chatBody.innerHTML += `
            <div style="text-align:left;margin:5px 0;">
                <span style="background:#eee;padding:6px 10px;border-radius:10px;">
                    ${data}
                </span>
            </div>
        `;
        chatBody.scrollTop = chatBody.scrollHeight;
    });

    input.value = "";
}

function toggleContact() {
    var box = document.getElementById("contactBox");
    box.style.display = box.style.display === "flex" ? "none" : "flex";
}

function sendContact() {

    var fullName = document.getElementById("fullName").value.trim();
    var phone = document.getElementById("phone").value.trim();
    var carName = document.getElementById("carName").value.trim();
    var message = document.getElementById("message").value.trim();

    if (fullName === "" || phone === "") {
        alert("Vui lòng nhập đầy đủ thông tin!");
        return;
    }

    fetch('/Contacts/Create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body:
            "FullName=" + encodeURIComponent(fullName) +
            "&Phone=" + encodeURIComponent(phone) +
            "&CarName=" + encodeURIComponent(carName) +
            "&Message=" + encodeURIComponent(message)
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            alert("Gửi thành công! Nhân viên sẽ liên hệ bạn.");

            document.getElementById("fullName").value = "";
            document.getElementById("phone").value = "";
            document.getElementById("carName").value = "";
            document.getElementById("message").value = "";

            toggleContact();
        }
        else {
            alert("Có lỗi xảy ra!");
        }
    })
    .catch(error => {
        alert("Lỗi kết nối server!");
        console.log(error);
    });
}