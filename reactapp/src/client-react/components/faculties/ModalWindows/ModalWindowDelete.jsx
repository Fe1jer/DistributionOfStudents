import React from 'react';

export default function ModalWindowDelete({ onDelete }) {
    const [shortName, setShortName] = React.useState("");
    const [fullName, setFullName] = React.useState("");

    const onSubmit = (e) => {
        e.preventDefault();
        onDelete(shortName);
    }
    React.useEffect(() => {
        var exampleModal = document.getElementById('facultyDeleteModalWindow')
        exampleModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = event.relatedTarget;
            // Extract info from data-bs-* attributes
            setShortName(button.getAttribute('data-bs-shortname'));
            setFullName(button.getAttribute('data-bs-fullname'));
        })
    }, [])
    return (
        <form onSubmit={onSubmit}>
            <div className="modal fade" id="facultyDeleteModalWindow" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Удалить <b className="text-success">{shortName}</b></h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>
                                Вы уверенны, что хотите удалить факультет?
                                <br />
                                <b className="text-success">{fullName}</b> будет удалён без возможности восстановления.
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
