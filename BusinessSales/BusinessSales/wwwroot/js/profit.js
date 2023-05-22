let customChart = document.getElementById("custom-chart").getContext("2d");

let programmingChart = new Chart(customChart, {
  type: "bar",
  data: {
    labels: ["Extra Small", "Small", "Medium", "Large", "Extra Large"],
    datasets: [
      {
        label: "# of Votes (in Thousands)",
        data: [8, 11, 17, 28, 36],
        backgroundColor: [
          "rgba(151, 185, 129, 1)",
          "rgba(90, 149, 117, 1)",
          "rgba(42, 112, 104, 1)",
          "rgba(6, 74, 85, 1)",
          "rgba(0, 39, 57, 1)",
        ],
        borderColor: [
          "rgba(151, 185, 129, 0.5)",
          "rgba(90, 149, 117, 0.5)",
          "rgba(42, 112, 104, 0.5)",
          "rgba(6, 74, 85, 0.5)",
          "rgba(0, 39, 57, 0.5)",
        ],
        borderWidth: 2,
      },
    ],
  },
  options: {
    scales: {
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
          },
        },
      ],
    },
    plugins: {
      legend: {
        display: false,
      },
    },
  },
});

const ul = document.querySelector(".details ul");

const populateUl = () => {
  programmingChart.data.labels.forEach((l, i) => {
    let li = document.createElement("li");
    li.innerHTML = `${l}: <span class='percentage'>${programmingChart.data.datasets[0].data[i]}</span>`;
    ul.appendChild(li);
  });
};

//populateUl();
const legendContainer = document.getElementById("legend-container");

const generateLegend = () => {
  while (legendContainer.firstChild) {
    legendContainer.firstChild.remove();
  }

  const legendUL = document.createElement("ul");
  legendUL.classList.add("legend");
  

  programmingChart.data.labels.forEach((label, index) => {
    const dataset = programmingChart.data.datasets[0];
    const li = document.createElement("li");
    
    const boxSpan = document.createElement("div");
    boxSpan.style.backgroundColor = dataset.backgroundColor[index];
    boxSpan.style.borderColor = dataset.borderColor[index];
    boxSpan.style.borderWidth = dataset.borderWidth + "px";
    boxSpan.style.display = "inline-block";

    const textContainer = document.createElement("span");

    const text = document.createTextNode(`${label} : ${dataset.data[index]}`);
    textContainer.appendChild(text);

    li.appendChild(boxSpan);
    li.appendChild(textContainer);
    legendUL.appendChild(li);
  });

  legendContainer.appendChild(legendUL);
};

generateLegend();
