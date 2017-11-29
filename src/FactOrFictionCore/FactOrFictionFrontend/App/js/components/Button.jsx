import React from 'react'
import PropTypes from 'prop-types';

const Button = (props) => (
    <button 
        className={"ff-Button change-view-button ms-Button"}
        onClickCapture={e => {
            e.stopPropagation();
            props.handleClick(); 
            return false;         
        }}>
        {props.content}
    </button>
);

Button.propTypes = {
    content: PropTypes.node.isRequired,
    handleClick: PropTypes.func.isRequired
}

export default Button;