import React from 'react'
import PropTypes from 'prop-types';

const Button = (props) => (
    <button 
        className={"ff-Button change-view-button ms-Button"}
        onClick={props.handleClick}>
        {props.text}
    </button>
);

Button.propTypes = {
    text: PropTypes.string.isRequired,
    handleClick: PropTypes.func.isRequired
}

export default Button;