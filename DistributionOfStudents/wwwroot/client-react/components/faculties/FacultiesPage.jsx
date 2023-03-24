import FacultyCard from './FacultyCard.jsx';
import FacultyCardPreloader from "./FacultyCardPreloader.jsx";
import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";

function FacultiesPage({ apiUrl }) {
    const [facultiesPlans, setFacultiesPlans] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8]

    const onDeleteFaculty = (facultyShortName) => {
        var xhr = new XMLHttpRequest();
        xhr.open("delete", apiUrl + '/' + facultyShortName, true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                loadData();
            }
        }.bind(this);
        xhr.send();
        $('#facultyDeleteModalWindow').modal('hide');
    }

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultiesPlans(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                <h1 className="placeholder-glow"><span className="placeholder" style={{ width: 220 }}></span></h1>
                <div className="row mx-1">{
                    numbers.map((item) =>
                        <FacultyCardPreloader key={"FacultyCardPreloader" + item} />
                    )}
                </div>
                <link rel="stylesheet" href="/css/faculty.css" type="text/css" />
            </div>
        );
    }
    else {
        return (
            <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                <h1 className="d-inline-block">Факультеты</h1>
                <a className="text-success ms-2" href="/Faculties/Create">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                    </svg>
                </a>
                <ModalWindowDelete apiUrl={apiUrl} onDelete={onDeleteFaculty} />
                <div className="row mx-1">{
                    facultiesPlans.map((item) =>
                        <FacultyCard key={item.shortName + item.fullName} faculty={item} />
                    )}
                </div>
                <link rel="stylesheet" href="/css/faculty.css" type="text/css" />
            </div>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<FacultiesPage apiUrl="/api/FacultiesApi" />);