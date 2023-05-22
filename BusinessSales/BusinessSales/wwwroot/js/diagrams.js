const chartData = {
    labels: ["Extra Small", "Small", "Medium", "Large", "Extra Large"],
    data: [8, 11, 17, 28, 36],
  };
  
  const myChart = document.querySelector(".my-chart");
  const ul = document.querySelector(".programming-stats .details ul");
  
  new Chart(myChart, {
    type: "doughnut",
    data: {
      labels: chartData.labels,
      datasets: [
        {
          label: "amount of products",
          data: chartData.data,
        },
      ],
    },
    options: {
      borderWidth: 10,
      borderRadius: 2,
      hoverBorderWidth: 0,
      plugins: {
        legend: {
          display: false,
        },
        layout: {
          padding: {
            top: 30,
            bottom: 30,
          },
        },
        datalabels: {
          color: "#023047",
          anchor: "end",
          align: "start",
          offset: -10,
          borderWidth: 2,
          borderColor: "#fff",
          borderRadius: 4,
          backgroundColor: "#fff",
          font: {
            size: 14,
            weight: "bold",
          },
          formatter: function (value, context) {
            return context.chart.data.labels[context.dataIndex];
          },
        },
      },
    },
  });
    
  const populateUl = () => {
    chartData.labels.forEach((l, i) => {
      let li = document.createElement("li");
      li.innerHTML = `${l}: <span class='percentage'>${chartData.data[i]}%</span>`;
      ul.appendChild(li);
    });
  };
  
  //populateUl();

  let customChart = document.getElementById("custom-chart").getContext("2d");
  
  let programmingChart = new Chart(customChart, {
  type: "bar",
  data: {
    labels: ["Extra Small", "Small", "Medium", "Large", "Extra Large"],
    datasets: [
      {
        data: [8, 11, 17, 28, 36],
        backgroundColor: [
          "rgba(0, 176, 255, 0.6)",
          "rgba(164, 118, 255, 0.6)",
          "rgba(255, 240, 0, 0.6)",
          "rgba(255, 0, 89, 0.6)",
          "rgba(12, 255, 65, 0.6)",
        ],
        borderColor: [
          "rgba(0, 176, 255, 1)",
          "rgba(164, 118, 255, 1)",
          "rgba(255, 240, 0, 1)",
          "rgba(255, 0, 89, 1)",
          "rgba(12, 255, 65, 1)",
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
    lagend:{
        display: true,
    }
  },
});
