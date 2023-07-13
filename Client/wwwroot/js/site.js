/*let btn1 = document.getElementById("btn-1");
let btn2 = document.getElementById("btn-2");
let btn3 = document.getElementById("btn-3");

let btn1changed = false;
let btn2changed = false;
let btn3changed = false;

btn1.onclick = () => {
    if (!btn1changed) {
        document.getElementById("div-1").innerHTML = "Data Baru";
        document.getElementById("div-1").style.backgroundColor = "blue";
        btn1changed = true;
    } else {
        document.getElementById("div-1").innerHTML = "Data Lama";
        document.getElementById("div-1").style.backgroundColor = "white";
        btn1changed = false;
    }
}

btn2.onclick = () => {
    if (!btn2changed) {
        document.getElementById("div-2").innerHTML = "Data Baru";
        document.getElementById("div-2").style.backgroundColor = "yellow";
        btn2changed = true;
    } else {
        document.getElementById("div-2").innerHTML = "Data Lama";
        document.getElementById("div-2").style.backgroundColor = "white";
        btn2changed = false;
    }
}

btn3.onclick = () => {
    if (!btn3changed) {
        document.getElementById("div-3").innerHTML = "Data Baru";
        document.getElementById("div-3").style.backgroundColor = "red";
        btn3changed = true;
    } else {
        document.getElementById("div-3").innerHTML = "Data Lama";
        document.getElementById("div-3").style.backgroundColor = "white";
        btn3changed = false;
    }
}

document.getElementById("light").onclick = () => {
    setInterval(function () {
        let colors = ["red", "green", "blue","yellow"];
        let randomIndex = Math.floor(Math.random() * colors.length);
        document.body.style.backgroundColor = colors[randomIndex];
    }, 50);
}

let arrayMhsObj = [
    { nama: "budi", nim: "a112015", umur: 20, isActive: true, fakultas: { name: "komputer" } },
    { nama: "joko", nim: "a112035", umur: 22, isActive: false, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112020", umur: 21, isActive: true, fakultas: { name: "komputer" } },
    { nama: "herul", nim: "a112032", umur: 25, isActive: true, fakultas: { name: "ekonomi" } },
    { nama: "herul", nim: "a112040", umur: 21, isActive: true, fakultas: { name: "komputer" } },
];

// 1
let fakultasKomputer = [];
for (i = 0; i < arrayMhsObj.length; i++) {
    if (arrayMhsObj[i]['fakultas']['name'] == 'komputer') {
        fakultasKomputer.push(arrayMhsObj[i]);
    }
}

console.log(fakultasKomputer);

// 2
let isActive = [];
for (i = 0; i < arrayMhsObj.length; i++) {
    isActive.push(arrayMhsObj[i]);
    if (parseInt(arrayMhsObj[i]['nim'].slice(-2)) >= 30) {
        isActive[i]['isActive'] = false;
    }
}

console.log(isActive);*/

/*$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon"
}).done((result) => {
    let temp = "";
    $.each(result.results, (key, val) => {
        temp += `<tr class="text-center">
                    <td>${key + 1}</td>
                    <td>${val.name}</td>
                    <td><button onclick="detail('${val.url}')" data-bs-toggle="modal" data-bs-target="#modalPokemon" class="btn btn-primary">Detail</button></td>
                </tr>`;
    })
    $("#tbodypokemon").html(temp);

});*/

$(document).ready(function () {
    $('#mytable').DataTable({
        ajax: {
            url: "https://pokeapi.co/api/v2/pokemon",
            dataType: "JSON",
            dataSrc: "results",
        },
        columns: [
            {
                data: null,
                render: (data, type, row, meta) => {
                    return meta.row + 1;
                }
            },
            { data: "name" },
            {
                data: "url",
                render: (data, type, row) => {
                    return `<button onclick="detail('${data}')" data-bs-toggle="modal" data-bs-target="#modalPokemon" class="btn btn-primary">Detail</button>`;
                }
            },
        ],
    });
});

function detail(stringURL) {
    $.ajax({
        url: stringURL
    }).done(res => {
        let temp = "";
        $.each(res.types, (key, val) => {
        let colors = ["bg-danger", "bg-success", "bg-primary", "bg-warning"];
        let randomIndex = Math.floor(Math.random() * colors.length);
            temp += `<div class="badge ${colors[randomIndex]}">${val.type.name}</div> `;
        });
        $("#types").html(temp);
        let content = "";
        $.each(res.abilities, (key, val) => {
            content += `<li>${val.ability.name}</li> `;
        });
        $('#content').html(content);
        $("#exampleModalLabel").html(res.name);
        $("#image-front").attr("src", res.sprites.front_default);
        $("#image-back").attr("src", res.sprites.back_default);
        $("#hp").css("width", res.stats[0].base_stat + "%").html("HP : " + res.stats[0].base_stat);
        $("#attack").css("width", res.stats[1].base_stat + "%").html("Attack : " + res.stats[1].base_stat);
        $("#defense").css("width", res.stats[2].base_stat + "%").html("Defense : " + res.stats[2].base_stat);
        $("#sattack").css("width", res.stats[3].base_stat + "%").html("Spesial Attack : " + res.stats[3].base_stat);
        $("#sdefense").css("width", res.stats[4].base_stat + "%").html("Spesial Defense : " + res.stats[4].base_stat);
        $("#speed").css("width", res.stats[5].base_stat + "%").html("Speed : " + res.stats[5].base_stat);
        console.log(res);
    })
};