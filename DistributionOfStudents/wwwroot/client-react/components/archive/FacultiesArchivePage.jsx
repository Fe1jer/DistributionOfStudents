import FacultyArchive from './FacultyArchive.jsx';
import FacultiesArchivePreloader from "./FacultiesArchivePreloader.jsx";

function FacultiesArchivePlans({ apiUrl, year, form }) {
    const [facultiesArchive, setFacultiesArchive] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2]

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + '/GetArchveByYearAndForm/' + year + "/" + form, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultiesArchive(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center"><span className="placeholder w-100"></span></h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <FacultiesArchivePreloader key={"FacultiesPlansPreloader" + item} />
                    )}
                </div>
            </React.Suspense>
        );
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">{form} за {year} год</h1>
                <div className="ps-lg-4 pe-lg-4 position-relative">{
                    facultiesArchive.map((item) =>
                        <FacultyArchive key={item.facultyFullName + year + form} facultyArchive={item} />
                    )}
                </div>
            </React.Suspense>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<FacultiesArchivePlans apiUrl="/api/ArchiveApi" form="Дневная форма получения образования за счет средств республиканского бюджета" year={2022} />);