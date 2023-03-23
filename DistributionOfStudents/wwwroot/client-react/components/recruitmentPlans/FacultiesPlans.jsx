import FacultyPlans from './FacultyPlans.jsx';
import FacultiesPlansPreloader from "./FacultiesPlansPreloader.jsx";
import ModalWindowDelete from "./ModalWindowDelete.jsx";

function FacultiesPlans({ apiUrl }) {
    const [facultiesPlans, setFacultiesPlans] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const [year, setYear] = React.useState(0);
    var numbers = [1, 2, 3, 4]

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultiesPlans(data);
            setLoading(false);
            const year = data.lenght != 0 ? Math.max(...data.map(o => o.year)) : 0;
            setYear(year);
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center"><span className="placeholder w-25"></span></h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <FacultiesPlansPreloader key={"FacultiesPlansPreloader" + item} />
                    )}
                </div>
            </React.Suspense>
        );
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center"> План приёма на {year} год</h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">
                    <ModalWindowDelete apiUrl={apiUrl} onLoadData={loadData} />{
                        facultiesPlans.map((item) =>
                            <FacultyPlans key={item.facultyShortName} facultyFullName={item.facultyFullName} facultyShortName={item.facultyShortName} year={item.year} plansForSpecialities={item.plansForSpecialities} />
                        )}
                </div>
            </React.Suspense>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<FacultiesPlans apiUrl="/api/RecruitmentPlansApi" />);