export default class StudentsApi {
    static baseUrl = "/api/StudentsApi/";

    static getStudents(searhText, currentPage, pageLimit) { return this.baseUrl + "?searchStudents=" + searhText + "&page=" + currentPage + "&pageLimit=" + pageLimit }
}