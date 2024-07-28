export default function getData(statistic) {
    return {
        labels: statistic.data.labels,
        datasets: statistic.data.datasets.map((plan, index) => ({
            label: plan.label,
            data: plan.data,
            tension: 0.1,
            borderColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
            fill: false,
            spanGaps: false
        })),
        options: {
            plugins: { legend: { display: false, }, }
        }
    };
}