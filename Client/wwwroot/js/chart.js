$.ajax({
    url: `https://localhost:7103/api/universities`,
    type: "GET",
    success: (results) => {
        universities = results.data;
        const university = universities.reduce(function (acc, result) {
            if (!acc.universities.includes(result.code)) {
                acc.universities.push(result.code);
                acc.count.push(1);
            } else {
                const index = acc.universities.indexOf(result.code);
                acc.count[index]++;
            }
            return acc;
        }, { universities: [], count: [] });

        chart(university)
    },
    error: (data) => {
        console.log("Error");
    }
});

function chart(data) {
    const labels = data.universities;
    const counts = data.count;

    const pie = document.getElementById("pieChart").getContext("2d");
    new Chart(pie, {
        type: "pie",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "University",
                    data: counts,
                },
            ],
        },
    });

    const bar = document.getElementById("barChart").getContext("2d");
    new Chart(bar, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "University",
                    data: counts,
                },
            ],
        },
    });

    const doughnut = document.getElementById("doughnutChart").getContext("2d");
    new Chart(doughnut, {
        type: "doughnut",
        data: {
            labels: labels,
            datasets: [
                {
                    label: "University",
                    data: counts,
                },
            ],
        },
    });
}

