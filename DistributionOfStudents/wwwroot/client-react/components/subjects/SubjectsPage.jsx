﻿import SubjectsList from "./SubjectsList.jsx";
import TablePreloader from "../TablePreloader.jsx";

function SubjectsPage({ apiUrl }) {
    const [subjects, setSubjects] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    // загрузка данных
    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setSubjects(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    React.useEffect(() => {
        loadData();
    }, [])
    if (loading) {
        return <React.Suspense>
            <h1 className="text-center"><span className="placeholder w-50"></span></h1>
            <hr />
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Список предметов для сдачи ЦТ и ЦЭ</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <SubjectsList apiUrl={apiUrl} key={subjects} subjects={subjects} loadData={loadData} />
                </div>
            </React.Suspense>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<SubjectsPage apiUrl="/api/SubjectsApi" />);
