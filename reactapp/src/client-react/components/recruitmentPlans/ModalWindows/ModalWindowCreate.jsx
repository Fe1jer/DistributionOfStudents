import React from 'react';

export default function CreateModalWindow({ onCreate }) {
    const [facultyShortName, setFacultyShortName] = React.useState("");
    const [year, setYear] = React.useState("");

    const onSubmit = (e) => {
        e.preventDefault();
        onCreate();
    }
    React.useEffect(() => {
        var exampleModal = document.getElementById('facultyPlansCreateModalWindow')
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
            <div className="modal fade" id="facultyPlansCreateModalWindow" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Создать план</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>
                                Перед тем как создать план приёма на <b className="text-success">{year} год</b>, проверьте заполнение всех ячеек плана приёма.
                            </p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                            <input type="submit" className="btn btn-primary" value="Сохранить" />
                        </div>
                    </div>
                </div>
            </div>
        </form>
    );
}
