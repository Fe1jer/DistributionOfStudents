import StudentsList from "./StudentsList.jsx";
import Search from "../Searh.jsx";
import Pagination from "../Pagination.jsx";
import TablePreloader from "../TablePreloader.jsx";

function StudentsPage({ apiUrl }) {
    const [students, setStudents] = React.useState([]);
    const [searhText, setSearhText] = React.useState("");
    const [currentPage, setCurrentPage] = React.useState(0);
    const [pageLimit, setPageLimit] = React.useState(30);
    const [countSearchStudents, setCountSearchStudents] = React.useState(0);
    const [pageNeighbours, setPageNeighbours] = React.useState(4);
    const [loading, setLoading] = React.useState(true);

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "?searchStudents=" + searhText + "&page=" + currentPage + "&pageLimit=" + pageLimit, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setStudents(data);
            countOfSearchStudents(searhText);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    const countOfSearchStudents = (searhText) => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "/CountOfSearchStudents?searchStudents=" + searhText, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setCountSearchStudents(data);
        }.bind(this);
        xhr.send();
    }
    const onSearhChange = (text) => {
        setSearhText(text);
    }
    const onPageChanged = (data) => {
        setCurrentPage(data.currentPage);
        setPageLimit(data.pageLimit);
    }
    React.useEffect(() => {
        loadData();
    }, [searhText, currentPage])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center"><span className="placeholder w-25"></span></h1>
            <hr />
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Все заявки абитуриентов</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <Search filter={onSearhChange} />
                    <StudentsList key={students} students={students} />
                    <Pagination key={countSearchStudents} totalRecords={countSearchStudents} pageLimit={pageLimit} pageNeighbours={pageNeighbours} onPageChanged={onPageChanged} />
                </div>
            </React.Suspense>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<StudentsPage apiUrl="/api/StudentsApi" />);