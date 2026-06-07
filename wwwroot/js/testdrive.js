var showrooms = [
    { name: "Toyota Bình Dương", lat: 10.9804, lng: 106.6519 },
    { name: "Toyota Thủ Đức", lat: 10.8491, lng: 106.7720 },
    { name: "Toyota Biên Hòa", lat: 10.9447, lng: 106.8243 }
];

function getDistance(lat1, lon1, lat2, lon2) {
    var R = 6371;
    var dLat = (lat2 - lat1) * Math.PI / 180;
    var dLon = (lon2 - lon1) * Math.PI / 180;

    var a =
        Math.sin(dLat / 2) ** 2 +
        Math.cos(lat1 * Math.PI / 180) *
        Math.cos(lat2 * Math.PI / 180) *
        Math.sin(dLon / 2) ** 2;

    return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
}

function loadShowrooms(selected = null) {
    let select = document.getElementById("showroomSelect");
    select.innerHTML = "";

    showrooms.forEach(s => {
        let opt = document.createElement("option");
        opt.value = s.name;
        opt.text = s.name;

        if (s.name === selected) opt.selected = true;

        select.appendChild(opt);
    });
}

// 📍 lấy vị trí
navigator.geolocation.getCurrentPosition(async function (pos) {

    try {
        let lat = pos.coords.latitude;
        let lng = pos.coords.longitude;

        let res = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}`);
        let data = await res.json();

        document.getElementById("locationBox").value =
            data.display_name || "Không xác định";

        let nearest = null;
        let min = 9999;

        showrooms.forEach(s => {
            let d = getDistance(lat, lng, s.lat, s.lng);
            if (d < min) {
                min = d;
                nearest = s;
            }
        });

        if (nearest) {
            loadShowrooms(nearest.name);

            let box = document.getElementById("suggestBox");
            box.style.display = "block";
            box.innerText = "📍 Showroom gần bạn nhất: " + nearest.name;
        }

    } catch (e) {
        console.log("Lỗi:", e);
        loadShowrooms();
    }

}, function () {
    document.getElementById("locationBox").value = "Không lấy được vị trí";
    loadShowrooms();
});