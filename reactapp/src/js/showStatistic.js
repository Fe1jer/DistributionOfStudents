import { getPlansStatistic, getGroupStatistic } from "./getDataFromDB.js";

function showStatistic(canvasId, statistic) {
    new Chart(canvasId, {
        type: "line",
        data: {
            labels: statistic.data.labels,
            datasets: statistic.data.datasets.map(plan => ({
                label: plan.label,
                data: plan.data,
                tension: 0.1,
                borderColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
                fill: false,
                spanGaps: false
            }))
        }
    });
}

export async function showPlansStatistic(canvasId, groupId, facultyName) {
    var plansStatistic = await getPlansStatistic(groupId, facultyName);
    showStatistic(canvasId, plansStatistic);
}

export async function showGroupStatistic(canvasId, groupId) {
    var groupStatistic = await getGroupStatistic(groupId);
    showStatistic(canvasId, groupStatistic);
}