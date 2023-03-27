export async function getPlansStatistic(groupId, facultyName) {
    const response = await fetch("/api/StatisticApi/PlansStatisticChart?groupId=" + groupId + "&facultyName=" + facultyName, {
        method: "GET"
    });
    let data = await response.json();
    return data;
}

export async function getGroupStatistic(groupId) {
    const response = await fetch("/api/StatisticApi/GroupStatisticChart?groupId=" + groupId, {
        method: "GET"
    });
    let data = await response.json();
    return data;
}