function SubjectDeleteForm({ apiUrl, onLoadData }) {
    const [name, setName] = React.useState("");
    const [id, setId] = React.useState("");

    const onSubmit = (e) => {
        if (id != "0") {
            e.preventDefault();
            var xhr = new XMLHttpRequest();
            xhr.open("delete", apiUrl + '/' + id, true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    $('#subjectDeleteForm').modal('hide');
                    onLoadData();
                    setName("");
                    setId("");
                }
            }.bind(this);
            xhr.send();
        }
    }
    React.useEffect(() => {
        var me = this;
        var exampleModal = document.getElementById('subjectDeleteForm')
        exampleModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = event.relatedTarget;
            // Extract info from data-bs-* attributes
            var subjectName = button.getAttribute('data-bs-subjectname');
            var id = button.getAttribute('data-bs-id');
            setName(subjectName);
            setId(id);

        })
    }, [])
    return (
        <form onSubmit={onSubmit}>
            <div className="modal fade" id="subjectDeleteForm" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Удалить <b className="text-success">{name}</b></h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <p>
                                Вы уверенны, что хотите удалить этот предмет?
                                <br />
                                Предмет <b className="text-success" id="deleteName">"{name}"</b> будет удалён без возможности восстановления.
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

function SubjectUpdateForm({ apiUrl, onLoadData }) {
    const [formValue, setFormValue] = React.useState({
        name: "",
        id: "",
    });
    const [errors, setErrors] = React.useState({});

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormValue((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    };
    const { name, id } = formValue;

    const onSubmit = (e) => {
        e.preventDefault();
        const jsonobj =
        {
            'Name': name.trim(),
            'Id': id,
        }
        var xhr = new XMLHttpRequest();
        if (id == "0") {
            xhr.open("post", apiUrl, true);
        }
        else {
            xhr.open("put", apiUrl + '/' + id, true);
        }
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                $('#subjectUpdateForm').modal('hide');
                onLoadData();
                setFormValue({
                    name: "",
                    id: ""
                })
            }
            if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                setErrors(a.obj[0].errors);
            }
        }.bind(this);
        xhr.send(JSON.stringify(jsonobj));
    }
    //добавление слушателя модальному окну с создание/изменением предмета
    React.useEffect(() => {
        var exampleModal = document.getElementById('subjectUpdateForm')
        exampleModal.addEventListener('show.bs.modal', function (event) {
            // Button that triggered the modal
            var button = event.relatedTarget;
            // Extract info from data-bs-* attributes
            var subjectName = button.getAttribute('data-bs-subjectname');
            var id = button.getAttribute('data-bs-id');
            // Update the modal's content.
            var modalTitle = exampleModal.querySelector('.modal-title');
            modalTitle.textContent = subjectName != "" ? 'Изменить ' + subjectName + ' на' : 'Создать предмет';
            setFormValue({
                name: subjectName,
                id: id
            })
            setErrors({});
        })
    }, [])
    const _formGroupErrors = (errors) => {
        var errorsName = "";
        if (errors) {
            errorsName += errors.join('\n');
        }
        return errorsName;
    }

    return (
        <form onSubmit={onSubmit}>
            <div className="modal fade" id="subjectUpdateForm" tabIndex="-1" role="dialog" aria-modal="true" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Создать предмет</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <div className="form-group pt-2">
                                <label className="control-label" htmlFor="Name">Название</label><sup>*</sup>
                                <input className="form-control" name="name" id="updateName" value={name} onChange={handleChange} type="text" />
                                <input id="updateId" value={id} name="id" type="hidden" onChange={handleChange} />
                                <span className="text-danger field-validation-valid" data-valmsg-for="Name" data-valmsg-replace="true">
                                    <span id="Faculty_FullName-error" className="">{_formGroupErrors(errors.Name)}</span>
                                </span>
                            </div>
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

export { SubjectUpdateForm, SubjectDeleteForm }