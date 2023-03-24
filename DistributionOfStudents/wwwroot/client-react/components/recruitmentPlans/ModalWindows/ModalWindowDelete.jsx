export default function DeleteModalWindow({ apiUrl, onDelete }) {
    const [facultyShortName, setFacultyShortName] = React.useState("");
    const [year, setYear] = React.useState("");

    const onSubmit = (e) => {
        e.preventDefault();
        onDelete(facultyShortName);
    }
    React.useEffect(() => {
        var exampleModal = document.getElementById('facultyPlansDeleteModalWindow')
        exampleModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = event.relatedTarget;
            // Extract info from data-bs-* attributes
            var facultyShortName = button.getAttribute('data-bs-facultyshortname');
            var year = button.getAttribute('data-bs-year');
            setFacultyShortName(facultyShortName);
            setYear(year);
        })
    }, [])
    return (
        <form onSubmit={onSubmit}>
            <div className="modal fade" id="facultyPlansDeleteModalWindow" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Удалить план за <b className="text-success">{year} год</b></h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>
                                Вы уверенны, что хотите удалить план приёма?
                                <br />
                                План приёма за <b className="text-success">{year} год</b> на <b className="text-success">{facultyShortName}</b> будет удалён без возможности восстановления.
                            </p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                            <input type="submit" className="btn btn-outline-danger" value="Удалить" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
