import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';

export default function Search({ filter, defaultValue, className }) {
    const onTextChanged = (e) => {
        var text = e.target.value.trim();   // удаляем пробелы
        filter(text); // передаем введенный текст в родительский компонент
    }
    return (
        <InputGroup className={className}>
            <Form.Control onChange={onTextChanged}
                autoComplete="off"
                type="search" defaultValue={defaultValue}
                placeholder="Найти" aria-label="Search"
            />
            <button className="input-group-text border-0" type="submit">
                <svg xmlns="http://www.w3.org/2000/svg" height="20" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                    <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                </svg>
            </button>
        </InputGroup>
    );
}