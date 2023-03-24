function ArchiveFormsPage({ apiUrl, year }) {
    const [forms, setForms] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2, 3, 4, 5]

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", apiUrl + "/GetArchveFormsByYear/" + year, true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setForms(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center"><span className="placeholder w-25"></span></h1>
            <hr className="mt-3 mx-0" />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                {numbers.map((number) =>
                    <a key={"ArchiveForm" + number} className="nav-link text-success p-0 mb-3 placeholder-glow"><span className="placeholder w-75"></span></a>
                )}
            </div>
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Архив</h1>
                <hr className="mt-3 mx-0" />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    {forms.map((form) =>
                        <a key={form} className="nav-link text-success p-0 mb-3" href={"/Archive/" + year + "/" + form}><h4>{form}</h4></a>
                    )}
                </div>
            </React.Suspense>
        );
    }
}

const container = document.getElementById('content');
const root = ReactDOM.createRoot(container);
root.render(<ArchiveFormsPage apiUrl="/api/ArchiveApi" year={2022} />);