const ul = document.querySelector(".details ul");
let customChart = document.getElementById("custom-chart").getContext("2d");
let programmingChart;

function setCostChart(graphData){
  programmingChart = new Chart(customChart, {
    type: "bar",
    data: {
      labels: graphData.info,
      datasets: [
        {
          label: "# of $",
          data: graphData.values,
          backgroundColor: graphData.colors,
          borderColor: graphData.colors,
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

  generateLegend();
}

const populateUl = () => {
  programmingChart.data.labels.forEach((l, i) => {
    let li = document.createElement("li");
    li.innerHTML = `${l}: <span class='percentage'>${
      programmingChart.data.datasets[0].data[i]}</span>`;
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
